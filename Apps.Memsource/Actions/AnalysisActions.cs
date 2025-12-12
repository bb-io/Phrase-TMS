using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Apps.PhraseTMS.Dtos.Analysis;
using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Analysis.Responses;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net;

namespace Apps.PhraseTMS.Actions;

[ActionList("Analysis")]
public class AnalysisActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
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

    [Action("Find analysis", Description = "Find a single analysis of a project using optional filters.")]
    public async Task<FullAnalysisDto> FindAnalysisAsync(
     [ActionParameter] ProjectRequest projectRequest,
     [ActionParameter] FindAnalysisQueryRequest input,
     [ActionParameter] ListAnalysesQueryRequest query)
    {
        var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/analyses"
            .WithQuery(query);

        var request = new RestRequest(endpoint, Method.Get);
        var analysisSummaries = await Client.Paginate<AnalysisDto>(request);

        if (!string.IsNullOrWhiteSpace(input.NameContains))
        {
            analysisSummaries = analysisSummaries
                .Where(a => a.Name != null &&
                            a.Name.Contains(input.NameContains, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (!analysisSummaries.Any())
            throw new PluginApplicationException("No analysis matched the provided filters.");

        var fullAnalyses = new List<FullAnalysisDto>();

        foreach (var summary in analysisSummaries)
        {
            var fullRequest = new RestRequest($"/api2/v3/analyses/{summary.UId}", Method.Get);
            var full = await Client.ExecuteWithHandling<FullAnalysisDto>(fullRequest);
            fullAnalyses.Add(full);
        }

        if (input.OnlyMostRecent == true)
        {
            fullAnalyses = fullAnalyses
                .OrderByDescending(a => a.DateCreated)
                .Take(1)
                .ToList();
        }

        if (!fullAnalyses.Any())
            throw new PluginApplicationException("No analysis matched the provided filters.");

        var selected = fullAnalyses
            .OrderByDescending(a => a.DateCreated)
            .First();

        return selected;
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
        [ActionParameter] WorkflowStepOptionalRequest workflowStepRequest,
        [ActionParameter] ListAllJobsQuery jobquery)
    {
        if (input.JobsUIds is null)
        {
            var endpoint = $"/api2/v2/projects/{projectRequest.ProjectUId}/jobs";
            var request2 = new RestRequest(endpoint.WithQuery(jobquery), Method.Get);
            if (workflowStepRequest.WorkflowStepId != null)
            {
                var workflowLevel = await Client.GetWorkflowstepLevel(projectRequest.ProjectUId, workflowStepRequest.WorkflowStepId);
                request2.AddQueryParameter("workflowLevel", workflowLevel);
            }

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
        fileName = $"{fileName}.{format.ToLower()}";
        var fileReference = await fileManagementClient.UploadAsync(memoryStream, MimeTypes.GetMimeType(fileName), fileName);

        return new AnalysisFileResponse { AnalysisFile = fileReference };
    }

    [Action("Update analysis", Description = "Assign provider and net rate scheme to analysis")]
    public async Task<FullAnalysisDto> UpdateAnalysis(
        [ActionParameter] GetAnalysisRequest input,
        [ActionParameter] EditAnalysisRequest analysisData)
    {
        var bodyDictionary = new Dictionary<string, object>();
        if (analysisData.vendorId != null)
        {
            bodyDictionary.Add("provider", new[] { new { type = "VENDOR", id = analysisData.vendorId } });
        }

        if (analysisData.userId != null)
        {
            bodyDictionary.Add("provider", new[] { new { type = "USER", id = analysisData.userId } });
        }

        if (!String.IsNullOrEmpty(analysisData.Name))
        {
            bodyDictionary.Add("name", analysisData.Name);
        }

        if (!String.IsNullOrEmpty(analysisData.NetRateSchemeId))
        {
            bodyDictionary.Add("netRateScheme", new { uid = analysisData.NetRateSchemeId });
        }

        var request = new RestRequest($"/api2/v2/analyses/{input.AnalysisUId}", Method.Put)
            .WithJsonBody(bodyDictionary, JsonConfig.DateSettings);
        return await Client.ExecuteWithHandling<FullAnalysisDto>(request);
    }

    [Action("Delete analyses", Description = "Delete analyses")]
    public async Task<DeleteAnalysesResponse> DeleteAnalyses(
    [ActionParameter] ProjectRequest projectRequest,
    [ActionParameter] JobRequest? jobRequest,
    [ActionParameter] DeleteAnalysesRequest input)
    {
        var idsToDelete = input.AnalysesIds?.ToList() ?? new List<string>();

        if (!idsToDelete.Any())
        {
            IEnumerable<AnalysisDto> analyses;

            if (jobRequest is not null && !string.IsNullOrEmpty(jobRequest.JobUId))
            {
                var endpoint =
                    $"/api2/v3/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}/analyses";

                var listRequest = new RestRequest(endpoint, Method.Get);
                analyses = await Client.Paginate<AnalysisDto>(listRequest);
            }
            else
            {
                var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/analyses";

                var listRequest = new RestRequest(endpoint, Method.Get);
                analyses = await Client.Paginate<AnalysisDto>(listRequest);
            }

            idsToDelete = analyses
                .Where(a => !string.IsNullOrEmpty(a.UId))
                .Select(a => a.UId!)
                .Distinct()
                .ToList();
        }

        var deleted = new List<string>();
        var errors = new List<DeleteAnalysisError>();

        foreach (var id in idsToDelete.Distinct())
        {
            var request = new RestRequest($"/api2/v1/analyses/{id}", Method.Delete);
            request.AddQueryParameter("purge", "true");

            try
            {
                await Client.ExecuteWithHandling(request);
                deleted.Add(id);
            }
            catch (Exception ex)
            {
                errors.Add(new DeleteAnalysisError
                {
                    AnalysisId = id,
                    Error = ex.Message
                });
            }
        }

        return new DeleteAnalysesResponse
        {
            DeletedAnalysesIds = deleted,
            Errors = errors
        };
    }
}