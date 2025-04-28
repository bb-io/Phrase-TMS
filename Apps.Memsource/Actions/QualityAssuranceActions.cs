using System.Text.RegularExpressions;
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.QualityAssurance.Responses;
using Apps.PhraseTMS.Models.QualityAssurance.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class QualityAssuranceActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Download LQA assessment", Description = "Downloads a single xlsx report based on specific job ID")]
    public async Task<DownloadLqaResponse> DownloadLqaAsync([ActionParameter] JobRequest jobRequest)
    {
        var request = new RestRequest("/api2/v1/lqa/assessments/reports", Method.Get)
            .AddParameter("jobParts", jobRequest.JobUId, ParameterType.QueryString);

        var response = await Client.ExecuteWithHandling(request);
        var stream = new MemoryStream(response.RawBytes!);
        stream.Position = 0;

        var fileName = GetFileNameFromResponse(response);
        var contentType = MimeTypes.GetMimeType(fileName);
        var fileReference = await fileManagementClient.UploadAsync(stream, contentType, fileName);
        return new()
        {
            LqaReport = fileReference
        };
    }

    [Action("Get LQA assessment", Description = "Get a specific LQA assessment")]
    public async Task<LQAAssessmentDto> GetLQAassessment([ActionParameter] JobRequest input)
    {
        var request = new RestRequest($"/api2/v1/lqa/assessments/{input.JobUId}", Method.Get);
        return await Client.ExecuteWithHandling<LQAAssessmentDto>(request);
    }

    [Action("Run auto LQA", Description = "Runs Auto LQA for specified job parts or all jobs in a given workflow step")]
    public Task RunAutoLQA([ActionParameter] ProjectRequest projectRequest, [ActionParameter] AutoLQARequest input)
    {
        if (input.JobsUIds is null && input.WorkflowLevel is null)
        {
            throw new PluginMisconfigurationException("Either Job IDs or Workflow step must be filled in");
        }

        if (input.JobsUIds != null && input.JobsUIds.Any() && input.WorkflowLevel != null)
        {
            throw new PluginMisconfigurationException(
                "One of the optional input values (Job IDs or Workflow step) must be filled in, not both");
        }

        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/runAutoLqa", Method.Post);

        var bodyDictionary = new Dictionary<string, object>();

        if (input.JobsUIds != null && input.JobsUIds.Any())
        {
            bodyDictionary.Add("jobParts", new[] { input.JobsUIds.Select(u => new { uid = u }) });
        }
        else if (input.WorkflowLevel != null)
        {
            bodyDictionary.Add("projectWorkflowStep", new { id = input.WorkflowLevel });
        }

        request.WithJsonBody(bodyDictionary);
        return Client.ExecuteWithHandling(request);
    }

    [Action("Run quality assurance", Description = "Run quality checks on job part")]
    public async Task<RunQAResponse> RunQA([ActionParameter] ProjectRequest project,
    [ActionParameter] JobRequest job,
    [ActionParameter] QaChecksRequest qachecks,
    [Display("Maximum warning count", Description = "Number between 1 and 100. Default is 100.")] double? MaxWarnings)
    {
        var request = new RestRequest($"/api2/v3/projects/{project.ProjectUId}/jobs/{job.JobUId}/qualityAssurances/run", Method.Post);
        if (qachecks != null)
        {
            var body = new { warningTypes = qachecks.WarningTypes.ToList(), maxQaWarningsCount = MaxWarnings is null? 100 : MaxWarnings };
            request.AddJsonBody(JsonConvert.SerializeObject(body));
        }
        else 
        {
            var body = new { maxQaWarningsCount = MaxWarnings is null ? 100 : MaxWarnings };
            request.AddJsonBody(JsonConvert.SerializeObject(body));
        }
        
        var response = await Client.ExecuteWithHandling<RunQAResponse>(request);
        response.OutstandingWarnings = response.SegmentWarnings != null && response.SegmentWarnings?.Count > 0;
        return response;
    }

    private static string GetFileNameFromResponse(RestResponse response)
    {
        var fileName = string.Empty;
        var contentDispositionHeader = response.Headers!
            .FirstOrDefault(x => x.Name?.Equals("Content-Disposition", StringComparison.OrdinalIgnoreCase) ?? false)
            ?.Value?.ToString();
        
        if (!string.IsNullOrWhiteSpace(contentDispositionHeader))
        {
            var fileNameStarMatch = Regex.Match(contentDispositionHeader, @"filename\*=UTF-8''(?<filename>[^;]+)");
            if (fileNameStarMatch.Success)
            {
                fileName = fileNameStarMatch.Groups["filename"].Value;
                fileName = Uri.UnescapeDataString(fileName);
            }
            else
            {
                var fileNameMatch = Regex.Match(contentDispositionHeader, @"filename=""?(?<filename>[^"";]+)""?");
                if (fileNameMatch.Success)
                {
                    fileName = fileNameMatch.Groups["filename"].Value;
                }
            }
        }

        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = $"Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
        }

        return fileName;
    }
}