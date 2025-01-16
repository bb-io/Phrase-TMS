using Apps.PhraseTMS.Models;
using Apps.PhraseTMS.Models.Async;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.PhraseTMS;

public class PhraseTmsClient : RestClient
{
    public PhraseTmsClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) :
        base(new RestClientOptions
            { BaseUrl = GetUri(authenticationCredentialsProviders) })
    {
    }

    public PhraseTmsClient(string baseUrl) : base(new RestClientOptions { BaseUrl = new(baseUrl) })
    {
    }

    public async Task<AsyncRequest?> PerformAsyncRequest(PhraseTmsRequest request,
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var asyncRequestResponse = await this.ExecuteWithHandling<AsyncRequestResponse>(request);
        if (asyncRequestResponse is null) return default;
        var asyncRequest = asyncRequestResponse.AsyncRequest;

        while (asyncRequest.AsyncResponse is null)
        {
            await Task.Delay(2000);
            var asyncStatusRequest = new PhraseTmsRequest($"/api2/v1/async/{asyncRequest.Id}", Method.Get,
                authenticationCredentialsProviders);
            asyncRequest = await this.ExecuteWithHandling<AsyncRequest>(asyncStatusRequest);
            if (asyncRequest is null) return default;
        }

        return asyncRequest;
    }

    public async Task<List<AsyncRequest>> PerformMultipleAsyncRequest(PhraseTmsRequest request,
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var result = new List<AsyncRequest>();
        var asyncRequestResponse = await ExecuteWithHandling<AsyncRequestMultipleResponse>(request);
        var asyncRequests = asyncRequestResponse.AsyncRequests;

        foreach (var asyncRequest in asyncRequests)
        {
            AsyncRequest asyncRequestSeparate = null;
            while (asyncRequestSeparate?.AsyncResponse is null)
            {
                await Task.Delay(2000);
                var asyncStatusRequest = new PhraseTmsRequest($"/api2/v1/async/{asyncRequest.AsyncRequest.Id}",
                    Method.Get, authenticationCredentialsProviders);
                asyncRequestSeparate = await ExecuteWithHandling<AsyncRequest>(asyncStatusRequest);
            }

            result.Add(asyncRequestSeparate);
        }

        return result;
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

        if (!string.IsNullOrEmpty(response.ErrorMessage))
        {
            if (response.ErrorMessage.Contains("User account inactive"))
                throw new PluginMisconfigurationException(response.ErrorMessage + "Please check your connection");

            throw new PluginApplicationException(response.ErrorMessage);
        }

        throw ConfigureErrorException(response.Content);
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

    private Exception ConfigureErrorException(string responseContent)
    {
        var error = JsonConvert.DeserializeObject<Error>(responseContent);

        if (error.ErrorDescription.Contains("JobCountLimit"))
            throw new PluginMisconfigurationException("You have reached your job count limit. Please remove some jobs or increase your limit by upgrading your Phrase plan.");

        if (error.ErrorDescription.Contains("targetLangs must match project"))
            throw new PluginMisconfigurationException("The target languages do not match the project. Please make sure the target languages in this action match the target languages of the project.").

        throw new PluginApplicationException(error.ErrorDescription);
    }
}