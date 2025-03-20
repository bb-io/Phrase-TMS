using Apps.PhraseTMS.Dtos.Async;
using Apps.PhraseTMS.Models;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Apps.PhraseTMS;

public class PhraseTmsClient : RestClient
{
    public PhraseTmsClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) :
        base(new RestClientOptions
            { BaseUrl = GetUri(authenticationCredentialsProviders) })
    {
        var authorization = authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
        this.AddDefaultHeader("Authorization", authorization);
        this.AddDefaultHeader("accept", "*/*");
    }

    public async Task<AsyncRequest?> PerformAsyncRequest(RestRequest request)
    {
        var asyncRequestResponse = await ExecuteWithHandling<AsyncRequestResponse>(request);
        if (asyncRequestResponse is null) return default;
        var asyncRequest = asyncRequestResponse.AsyncRequest;

        while (asyncRequest.AsyncResponse is null)
        {
            await Task.Delay(2000);
            var asyncStatusRequest = new RestRequest($"/api2/v1/async/{asyncRequest.Id}", Method.Get);
            asyncRequest = await ExecuteWithHandling<AsyncRequest>(asyncStatusRequest);
            if (asyncRequest is null) return default;
        }

        return asyncRequest;
    }

    public async Task<IEnumerable<T>> PerformMultipleAsyncRequest<T>(RestRequest request) where T : AsyncRequestArrayItem
    {
        var asyncRequestResponse = await ExecuteWithHandling<AsyncRequestMultipleResponse<T>>(request);
        var asyncRequests = asyncRequestResponse.AsyncRequests;

        foreach (var asyncRequest in asyncRequests)
        {
            AsyncRequest asyncRequestSeparate = null;
            while (asyncRequestSeparate?.AsyncResponse is null)
            {
                await Task.Delay(2000);
                var asyncStatusRequest = new RestRequest($"/api2/v1/async/{asyncRequest.AsyncRequest.Id}", Method.Get);
                asyncRequestSeparate = await ExecuteWithHandling<AsyncRequest>(asyncStatusRequest);
            }
        }

        return asyncRequests;
    }

    private static Uri GetUri(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var url = authenticationCredentialsProviders.First(p => p.KeyName == "url").Value;
        return new(url.TrimEnd('/') + "/web");
    }

    public async Task<T> ExecuteWithHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithHandling(request);
        return JsonConvert.DeserializeObject<T>(response.Content);
    }

    public async Task<RestResponse> ExecuteWithHandling(RestRequest request)
    {
        var response = await ExecuteAsync(request);

        if (response.IsSuccessful)
            return response;

        throw ConfigureErrorException(response);
    }
        
    public async Task<List<T>> Paginate<T>(RestRequest request)
    {
        var pageNumber = 0;
        int totalPages;
            
        var resource = request.Resource;
        var result = new List<T>();

        do
        {
            request.Resource = resource.SetQueryParameter("pageNumber", pageNumber++.ToString());

            var response = await ExecuteWithHandling<PaginationResponse<T[]>>(request);
            totalPages = response.TotalPages;

            result.AddRange(response.Content);
        } while (pageNumber < totalPages);

        return result;
    }

    public async Task<IEnumerable<T>> PaginateOnce<T>(RestRequest request)
    {
        var response = await ExecuteWithHandling<PaginationResponse<T[]>>(request);
        return response.Content;
    }

    private Exception ConfigureErrorException(RestResponse restResponse)
    {
        if (restResponse.Content == null)
            throw new PluginApplicationException(restResponse.ErrorMessage);

        Error? error;
        try
        {
            error = JsonConvert.DeserializeObject<Error>(restResponse.Content);
        }
        catch (Exception ex)
        {
            throw new Exception($"Content: {restResponse.Content}, Exception message: {ex.Message}");
        }
       

        if (string.IsNullOrEmpty(error.ErrorDescription))
        {
            throw new PluginApplicationException("There has been an error with no error description.");
        }

        if (restResponse.StatusCode.Equals(HttpStatusCode.Unauthorized))
        {
            throw new PluginMisconfigurationException("The connection is unauthorized. Please check your connection settings.");
        }

        if (restResponse.StatusCode.Equals(HttpStatusCode.NotFound))
        {
            throw new PluginApplicationException(error.ErrorDescription + " Please check the inputs for this action");
        }


        if (error.ErrorDescription.Contains("JobCountLimit"))
            throw new PluginMisconfigurationException("You have reached your job count limit. Please remove some jobs or increase your limit by upgrading your Phrase plan.");

        if (error.ErrorDescription.Contains("targetLangs must match project"))
            throw new PluginMisconfigurationException("The target languages do not match the project. Please make sure the target languages in this action match the target languages of the project.");

        if (error.ErrorDescription.Contains("contains unsupported locale."))
            throw new PluginMisconfigurationException(error.ErrorDescription);
        throw new PluginApplicationException(error.ErrorDescription);
    }
}