using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Analysis.Responses;
using Apps.PhraseTMS.Models.Async;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class AnalysisActions
{
    private readonly IFileManagementClient _fileManagementClient;
    
    public AnalysisActions(IFileManagementClient fileManagementClient)
    {
        _fileManagementClient = fileManagementClient;
    }
    
    [Action("List analyses", Description = "List all job's analyses")]
    public async Task<ListAnalysesResponse> ListAnalyses(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] ListAnalysesPathRequest path,
        [ActionParameter] ListAnalysesQueryRequest query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/jobs/{path.JobUId}/analyses"
            .WithQuery(query);
            
        var request = new PhraseTmsRequest(endpoint,
            Method.Get, authenticationCredentialsProviders);
        var response = await client.Paginate<AnalysisDto>(request);

        return new ListAnalysesResponse
        {
            Analyses = response
        };
    }

    [Action("Get analysis", Description = "Get job's analysis")]
    public Task<AnalysisDto> GetAnalysis(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] GetJobRequest jobRequest,
        [ActionParameter] GetAnalysisRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v3/analyses/{input.AnalysisUId}", Method.Get,
            authenticationCredentialsProviders);
        return client.ExecuteWithHandling<AnalysisDto>(request);
    }

    [Action("Create analysis", Description = "Create a new analysis")]
    public async Task<AsyncRequest> CreateAnalysis(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] CreateAnalysisInput input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v2/analyses", Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(new CreateAnalysisRequest(input),  JsonConfig.Settings);
            
        var asyncRequest = await client.PerformMultipleAsyncRequest(request, authenticationCredentialsProviders);
        return asyncRequest.First();
    }
    
    [Action("Download analysis", Description = "Download analysis")]
    public async Task<FileReference> DownloadAnalysis(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter, Display("Analysis")] string analysisUId,
        [ActionParameter, Display("Format"), DataSource(typeof(FormatDataHandler))] string? format)
    {
        format ??= "CSV";
        var client = new PhraseTmsClient("https://cloud.memsource.com/web");
        var request = new PhraseTmsRequest($"/api2/v1/analyses/{analysisUId}/download?format={format}", Method.Get, authenticationCredentialsProviders)
            .AddHeader("Accept", "application/octet-stream");
            
        var response = await client.ExecuteWithHandling(request);
        var bytes = response.RawBytes;
        
        if (bytes is not null)
        {
            var memoryStream = new MemoryStream(bytes);
            string fileName = $"analysis_{analysisUId}.{format}";
            var fileReference = await _fileManagementClient.UploadAsync(memoryStream, MimeTypes.GetMimeType(fileName), fileName);
            
            return fileReference;
        }
        
        throw new Exception("Failed to download analysis");
    }
}