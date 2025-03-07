using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Analysis.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Dtos.Analysis;
using Apps.PhraseTMS.Dtos.Jobs;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class AnalysisActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient): PhraseInvocable(invocationContext)
{

    [Action("Search job analyses", Description = "Search through all analyses of a specific job")]
    public async Task<ListAnalysesResponse> ListAnalyses(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ListAnalysesQueryRequest query)
    {
        var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}/analyses"
            .WithQuery(query);

        var request = new RestRequest(endpoint, Method.Get);
        var response = await Client.Paginate<AnalysisDto>(request);

        var result = new List<FullAnalysisDto>();

        foreach (var analysis in response)
        {
            var fullAnalysisRequest = new RestRequest($"/api2/v3/analyses/{analysis.UId}", Method.Get);
            var analysisResult = await Client.ExecuteWithHandling<FullAnalysisDto>(fullAnalysisRequest);
            result.Add(analysisResult);
        }

        return new ListAnalysesResponse { Analyses = result };
    }

    [Action("Search project analyses", Description = "Search through all analyses of a specific project")]
    public async Task<ListAnalysesResponse> ListProjectAnalyses(
    [ActionParameter] ProjectRequest projectRequest,
    [ActionParameter] ListAnalysesQueryRequest query)
    {
        var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/analyses"
            .WithQuery(query);

        var request = new RestRequest(endpoint, Method.Get);
        var response = await Client.Paginate<AnalysisDto>(request);

        var result = new List<FullAnalysisDto>();

        foreach (var analysis in response)
        {
            var fullAnalysisRequest = new RestRequest($"/api2/v3/analyses/{analysis.UId}", Method.Get);
            var analysisResult = await Client.ExecuteWithHandling<FullAnalysisDto>(fullAnalysisRequest);
            result.Add(analysisResult);
        }

        return new ListAnalysesResponse { Analyses = result };
    }

    [Action("Get analysis data", Description = "Returns the full details of a specific analysis")]
    public async Task<FullAnalysisDto> GetJobAnalysis(
        [ActionParameter] GetAnalysisRequest input)
    {
        var request = new RestRequest($"/api2/v3/analyses/{input.AnalysisUId}", Method.Get);
        return await Client.ExecuteWithHandling<FullAnalysisDto>(request);
    }

    [Action("Create analyses", Description = "Create one or multiple analyses for jobs in a given project")]
    public async Task<ListAnalysesResponse> CreateAnalysis(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] CreateAnalysisInput input,
        [ActionParameter] ListAllJobsQuery jobquery)
    {
        if (input.JobsUIds is null)
        {
            var endpoint = $"/api2/v2/projects/{projectRequest.ProjectUId}/jobs";
            var request2 = new RestRequest(endpoint.WithQuery(jobquery), Method.Get);

            var response = await Client.Paginate<ListJobDto>(request2);
            input.JobsUIds = response.Select(x => x.Uid).ToList();
        }

        var request = new RestRequest($"/api2/v2/analyses", Method.Post);
        request.WithJsonBody(new CreateAnalysisRequest(input), JsonConfig.Settings);

        var asyncResponse = await Client.PerformMultipleAsyncRequest<CreateAnalysisDto>(request);

        var result = new List<FullAnalysisDto>();

        foreach (var analysis in asyncResponse)
        {
            var fullAnalysisRequest = new RestRequest($"/api2/v3/analyses/{analysis.Analyse.Id}", Method.Get);
            var analysisResult = await Client.ExecuteWithHandling<FullAnalysisDto>(fullAnalysisRequest);
            result.Add(analysisResult);
        }
        return new ListAnalysesResponse { Analyses = result };
    }

    [Action("Download analysis file", Description = "Download analysis file in specified format")]
    public async Task<AnalysisFileResponse> DownloadAnalysis(
        [ActionParameter] GetAnalysisRequest analysisRequest,
        [ActionParameter, Display("Format"), StaticDataSource(typeof(FormatDataHandler))]
        string? format,
        [ActionParameter, Display("File name", Description = "File name without format. F.e.: analysis")]
        string? fileName)
    {
        format ??= "CSV";
        var request = new RestRequest($"/api2/v1/analyses/{analysisRequest.AnalysisUId}/download?format={format}", Method.Get)
            .AddHeader("Accept", "application/octet-stream");

        var response = await Client.ExecuteWithHandling(request);
        var bytes = response.RawBytes;

        var memoryStream = new MemoryStream(bytes);
        fileName ??= $"analysis_{analysisRequest.AnalysisUId}";
        fileName = $"{fileName}.{format}";
        var fileReference = await fileManagementClient.UploadAsync(memoryStream, MimeTypes.GetMimeType(fileName), fileName);

        return new AnalysisFileResponse { AnalysisFile = fileReference };
    }
}