using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
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
using Blackbird.Applications.Sdk.Common.Dictionaries;
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

        return new()
        {
            Analyses = response
        };
    }

    [Action("Get analysis", Description = "Get analysis")]
    public Task<AnalysisDto> GetAnalysis(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] GetAnalysisRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v3/analyses/{input.AnalysisUId}", Method.Get,
            authenticationCredentialsProviders);
        return client.ExecuteWithHandling<AnalysisDto>(request);
    }

    [Action("Get job analysis", Description = "Get job's analysis details")]
    public JobAnalysisResponse GetJobAnalysis(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] GetAnalysisRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/analyses/{input.AnalysisUId}/jobs/{jobRequest.JobUId}", Method.Get,
            authenticationCredentialsProviders);
        var response = client.ExecuteWithHandling<JobAnalysisDto>(request).Result;
        return new JobAnalysisResponse
        {
            Filename = response.FileName,
            TotalWords = response.data.all.words,
            Repetitions = response.data.repetitions.words,
            MemoryMatch101 = response.data.transMemoryMatches.match101.words,
            MemoryMatch100 = response.data.transMemoryMatches.match100.words,
            MemoryMatch95 = response.data.transMemoryMatches.match95.words,
            MemoryMatch85 = response.data.transMemoryMatches.match85.words,
            MemoryMatch75 = response.data.transMemoryMatches.match75.words,
            MemoryMatch50 = response.data.transMemoryMatches.match50.words,
            MemoryMatch0 = response.data.transMemoryMatches.match0.words,
            TotalInternalFuzzy = response.data.internalFuzzyMatches.match95.words + response.data.internalFuzzyMatches.match100.words + response.data.internalFuzzyMatches.match85.words + response.data.internalFuzzyMatches.match75.words + response.data.internalFuzzyMatches.match50.words + response.data.internalFuzzyMatches.match0.words,
            TotalMT = response.data.machineTranslationMatches.match95.words + response.data.machineTranslationMatches.match100.words + response.data.machineTranslationMatches.match85.words + response.data.machineTranslationMatches.match75.words + response.data.machineTranslationMatches.match50.words + response.data.machineTranslationMatches.match0.words,
            TotalNonTranslatable = response.data.nonTranslatablesMatches.match100.words + response.data.nonTranslatablesMatches.match99.words
        };
    }

    [Action("Create analysis", Description = "Create a new analysis")]
    public async Task<AsyncRequest> CreateAnalysis(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] CreateAnalysisInput input,
        [ActionParameter] ListAllJobsQuery jobquery)
    {

        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        if (input.JobsUIds is null)
        {
            var endpoint = $"/api2/v2/projects/{projectRequest.ProjectUId}/jobs";
            var request2 = new PhraseTmsRequest(endpoint.WithQuery(jobquery), Method.Get,
                authenticationCredentialsProviders);

            var response = await client.Paginate<JobDto>(request2);
            input.JobsUIds = response.Select(x => x.Uid).ToList();
        }

        var request = new PhraseTmsRequest($"/api2/v2/analyses", Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(new CreateAnalysisRequest(input), JsonConfig.Settings);

        var asyncRequest = await client.PerformMultipleAsyncRequest(request, authenticationCredentialsProviders);
        return asyncRequest.First();
    }

    [Action("Download analysis file", Description = "Download analysis file in specified format")]
    public async Task<FileReference> DownloadAnalysis(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] DownloadAnalysisRequest analysisRequest,
        [ActionParameter, Display("Format"), StaticDataSource(typeof(FormatDataHandler))]
        string? format,
        [ActionParameter, Display("File name", Description = "File name without format. F.e.: analysis"),
         StaticDataSource(typeof(FormatDataHandler))]
        string? fileName)
    {
        format ??= "CSV";
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/analyses/{analysisRequest.AnalysisUId}/download?format={format}",
                Method.Get, authenticationCredentialsProviders)
            .AddHeader("Accept", "application/octet-stream");

        var response = await client.ExecuteWithHandling(request);
        var bytes = response.RawBytes;

        if (bytes is not null)
        {
            var memoryStream = new MemoryStream(bytes);
            fileName ??= $"analysis_{analysisRequest.AnalysisUId}.{format}";
            var fileReference =
                await _fileManagementClient.UploadAsync(memoryStream, MimeTypes.GetMimeType(fileName), fileName);

            return fileReference;
        }

        throw new("Failed to download analysis");
    }
}