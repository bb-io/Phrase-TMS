using System.Text;
using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Dtos.Analysis;
using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Helpers;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Analysis.Responses;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Analysis.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.PhraseTMS.Actions;

[ActionList("Analysis")]
public class AnalysisActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Search job analyses", Description = "Search analyses for a specific job")]
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

    [Action("Find analysis", Description = "Find a single project analysis using optional filters")]
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
            return new FullAnalysisDto();

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
            return new FullAnalysisDto();

        var selected = fullAnalyses
            .OrderByDescending(a => a.DateCreated)
            .First();

        return selected;
    }

    [Action("Search project analyses", Description = "Search analyses for a specific project")]
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

    [Action("Get analysis data", Description = "Get full details for a specific analysis")]
    public async Task<FullAnalysisDto> GetJobAnalysis(
        [ActionParameter] GetAnalysisRequest input)
    {
        var request = new RestRequest($"/api2/v3/analyses/{input.AnalysisUId}", Method.Get);
        return await Client.ExecuteWithHandling<FullAnalysisDto>(request);
    }

    [Action("Create analyses", Description = "Create one or more analyses for jobs in a project")]
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

    [Action("Download analysis file", Description = "Download an analysis file")]
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
    
    [Action("Export project analysis", Description = 
        "Get raw and normalized project analysis JSON output. " +
        "The output of this action can be used by another app that supports importing analysis data")]
    public async Task<ExportProjectAnalysisResponse> ExportProjectAnalysis(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] OptionalAnalysisRequest analysisRequest,
        [ActionParameter] ExportProjectAnalysisRequest exportProjectAnalysisRequest)
    {
        List<string> analysisIdsToDownload = [];

        if (!string.IsNullOrWhiteSpace(analysisRequest.AnalysisUId))
            analysisIdsToDownload.Add(analysisRequest.AnalysisUId);
        else
        {
            var jobsRequest = new RestRequest($"/api2/v2/projects/{projectRequest.ProjectUId}/jobs");
            var jobsResponse = await Client.Paginate<ListJobDto>(jobsRequest);
            
            var allJobUids = jobsResponse.Select(x => x.Uid).ToList();
            if (allJobUids.Count == 0)
                throw new PluginMisconfigurationException("The project has no jobs to analyze.");

            var payload = new 
            {
                jobs = allJobUids.Select(uid => new { uid }).ToList(),
                analyzeByProvider = false
            };
            var createRequest = new RestRequest("/api2/v2/analyses", Method.Post)
                .WithJsonBody(payload, JsonConfig.Settings);

            var asyncResponse = await Client.PerformMultipleAsyncRequest<CreateAnalysisDto>(createRequest);
            var asyncResponseList = asyncResponse.ToList();
            
            if (asyncResponseList.Count == 0)
                 throw new PluginMisconfigurationException("Analysis creation failed or returned empty.");

            analysisIdsToDownload = asyncResponseList.Select(x => x.Analyse.Id).ToList();
        }

        if (analysisIdsToDownload.Count == 0)
            throw new PluginMisconfigurationException("No analyses could be found or created for this project.");

        List<string> mtEnabledLanguages = [];
        double? tmThreshold = null;

        if (exportProjectAnalysisRequest.CalculateSyntheticMtBucket is true)
        {
            mtEnabledLanguages = await FetchEnabledMtLanguages(projectRequest.ProjectUId);
            tmThreshold = await FetchTmThreshold(projectRequest.ProjectUId);
        }

        var downloadTasks = analysisIdsToDownload.Select(async analysisId =>
        {
            var downloadRequest = new RestRequest($"/api2/v1/analyses/{analysisId}/download?format=JSON")
                .AddHeader("Accept", "application/octet-stream");

            var downloadResponse = await Client.ExecuteWithHandling(downloadRequest);
            var bytes = downloadResponse.RawBytes;
            if (bytes == null)
                return [];

            string jsonString = Encoding.UTF8.GetString(bytes);
            JObject root = JObject.Parse(jsonString);

            if (root["analyseLanguageParts"] is not JArray languageParts) 
                return [];
                
            if (exportProjectAnalysisRequest.CalculateSyntheticMtBucket is true)
                ApplySyntheticMtHack(languageParts, mtEnabledLanguages, tmThreshold);

            return AnalysisHelper.GenerateAnalysis(languageParts);
        });

        List<Analysis>[] nestedResults = await Task.WhenAll(downloadTasks);
        List<Analysis> masterAnalysisList = nestedResults.SelectMany(list => list).ToList();
        
        string outputJsonString = JsonConvert.SerializeObject(masterAnalysisList, Formatting.Indented);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(outputJsonString));

        var fileName = $"analysis_{projectRequest.ProjectUId}.json";
        var fileReference = await fileManagementClient.UploadAsync(stream, "application/json", fileName);
        return new(fileReference);
    }

    [Action("Update analysis", Description = "Update provider and net rate scheme for an analysis")]
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

    [Action("Delete analyses", Description = "Delete one or more analyses")]
    public async Task<DeleteAnalysesResponse> DeleteAnalyses(
    [ActionParameter] ProjectRequest projectRequest,
    [ActionParameter, Display("Job ID"), DataSource(typeof(JobDataHandler))] string? jobId,
    [ActionParameter] DeleteAnalysesRequest input)
    {
        var idsToDelete = input.AnalysesIds?.ToList() ?? new List<string>();

        if (!idsToDelete.Any())
        {
            IEnumerable<AnalysisDto> analyses;

            if (!string.IsNullOrEmpty(jobId))
            {
                var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/jobs/{jobId}/analyses";
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


    private async Task<double> FetchTmThreshold(string projectUid)
    {
        var preTranslateReq = new RestRequest($"/api2/v4/projects/{projectUid}/preTranslateSettings");
        var preTranslateInfo = await Client.ExecuteWithHandling<PretranslateSettingsDto>(preTranslateReq);
        return preTranslateInfo.TmSettings.TranslationMemoryThreshold;
    }
    
    private async Task<List<string>> FetchEnabledMtLanguages(string projectUid)
    {
        var projectInfoReq = new RestRequest($"/api2/v1/projects/{projectUid}");
        var projectInfo = await Client.ExecuteWithHandling<ProjectDetailsDto>(projectInfoReq);

        if (projectInfo.MtSettings.Count == 0)
            return [];
        
        return projectInfo.MtSettings
            .Select(x => x.TargetLang)
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();
    }
    
    private static void ApplySyntheticMtHack(JArray languageParts, List<string> mtEnabledLanguages, double? tmThreshold)
    {
        if (tmThreshold == null || mtEnabledLanguages.Count == 0) return;

        var targetBuckets = new List<string> { "match0" };
        if (tmThreshold >= 0.50) 
            targetBuckets.Add("match50");
        if (tmThreshold >= 0.75)
            targetBuckets.Add("match75");
        if (tmThreshold >= 0.85) 
            targetBuckets.Add("match85");
        if (tmThreshold >= 0.95) 
            targetBuckets.Add("match95");

        foreach (var langPart in languageParts)
        {
            var targetLang = langPart["targetLang"]?.ToString();
            if (string.IsNullOrEmpty(targetLang)) 
                continue;

            bool hasMt = mtEnabledLanguages.Any(x => x.Equals(targetLang, StringComparison.OrdinalIgnoreCase));
            if (!hasMt) 
                continue;

            if (langPart["data"] is not JObject dataNode) 
                continue;

            foreach (var bucket in targetBuckets)
            {
                if (dataNode[bucket]?["words"] is not JObject wordsObject) 
                    continue;
                
                decimal tmValue = wordsObject["tm"]?.Value<decimal>() ?? 0m;
                wordsObject["tm"] = 0.0m;
                wordsObject["machine_translated"] = tmValue;
            }
        }
    }
}
