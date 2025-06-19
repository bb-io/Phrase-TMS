using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Dtos.Async;
using Apps.PhraseTMS.Models;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using HtmlAgilityPack;
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
        int[] retryDelaysInMs = { 2000, 4000, 8000 };
        RestResponse response = null;
        
        for (int attempt = 0; attempt <= retryDelaysInMs.Length; attempt++)
        {
            response = await ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response;
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError && attempt < retryDelaysInMs.Length)
            {
                await Task.Delay(retryDelaysInMs[attempt]);
                continue;
            }
            
            throw ConfigureErrorException(response);
        }

        return response!;
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
        if (restResponse.Content == null || !restResponse.Content.Any())
            throw new PluginApplicationException("Response content from server is empty. Please check the API response and try again." +  
                restResponse?.StatusCode != null ? restResponse?.StatusCode.ToString() : "" + " " + restResponse.ErrorMessage != null ? restResponse?.ErrorMessage.ToString() : "");

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

        if (restResponse.ContentType?.Contains("text/html", StringComparison.OrdinalIgnoreCase) == true || restResponse.Content.StartsWith("<"))
        {
            var errorMessage = ExtractHtmlErrorMessage(restResponse.Content);
            throw new PluginApplicationException(errorMessage);
        }


        if (error.ErrorDescription.Contains("JobCountLimit"))
        {
            throw new PluginMisconfigurationException("You have reached your job count limit. Please remove some jobs or increase your limit by upgrading your Phrase plan.");
        }

        if (error.ErrorDescription.Contains("targetLangs must match project"))
        {
            throw new PluginMisconfigurationException("The target languages do not match the project. Please make sure the target languages in this action match the target languages of the project.");
        }

        if (error.ErrorDescription.Contains("contains unsupported locale."))
        {
            throw new PluginMisconfigurationException(error.ErrorDescription);
        }

        throw new PluginApplicationException($"({error.ErrorCode}): {error.ErrorDescription}");
    }

    public async Task<int> GetWorkflowstepLevel(string projectId, string workflowStepId)
    {
        var request = new RestRequest($"/api2/v1/projects/{projectId}", Method.Get);
        var response = await ExecuteWithHandling<ProjectDto>(request);
        if (response.WorkflowSteps.Count() == 0) return 1;
        var workflow = response.WorkflowSteps.FirstOrDefault(x => x.InnerWorkflowStep.Id == workflowStepId);
        if (workflow == null) throw new PluginMisconfigurationException("The workflow step selected does not exist on the current project. Please select an existing workflow step or leave this input empty.");
        return workflow.WorkflowLevel;
    }

    public async Task<int> GetLastWorkflowstepLevel(string projectId)
    {
        var request = new RestRequest($"/api2/v1/projects/{projectId}", Method.Get);
        var response = await ExecuteWithHandling<ProjectDto>(request);
        if (response.WorkflowSteps.Count() == 0) return 1;
        return response.WorkflowSteps.Select(x => x.WorkflowLevel).Max();
    }

    private string ExtractHtmlErrorMessage(string html)
    {
        if (string.IsNullOrEmpty(html)) return "N/A";

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
        var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

        var title = titleNode?.InnerText.Trim() ?? "No Title";
        var body = bodyNode?.InnerText.Trim() ?? "No Description";
        return $"{title}: \nError Description: {body}";
    }
}