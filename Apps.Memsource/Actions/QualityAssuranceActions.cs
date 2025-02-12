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

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class QualityAssuranceActions(IFileManagementClient fileManagementClient)
{
    [Action("Download LQA assessment", Description = "Downloads a single xlsx report based on specific job ID")]
    public async Task<DownloadLqaResponse> DownloadLqaAsync(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest)
    {
        var credentialsProviders = authenticationCredentialsProviders as AuthenticationCredentialsProvider[] ??
                                   authenticationCredentialsProviders.ToArray();
        
        var client = new PhraseTmsClient(credentialsProviders);
        var request = new PhraseTmsRequest("/api2/v1/lqa/assessments/reports", Method.Get, credentialsProviders)
            .AddParameter("jobParts", jobRequest.JobUId, ParameterType.QueryString);

        var response = await client.ExecuteWithHandling(request);
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
    
    [Action("Add ignored warning", Description = "Add a new ignored warning to the job segment")]
    public Task AddIgnoredWarning(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] AddIgnoredWarningRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest(
            $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}/qualityAssurances/ignoredWarnings",
            Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            segments = new[]
            {
                new
                {
                    uid = input.SegmentUId,
                    warnings = new[] { new { id = input.WarningId } }
                }
            }
        });
        return client.ExecuteWithHandling(request);
    }

    [Action("List LQA profiles", Description = "List all LQA profiles")]
    public async Task<ListLqaProfilesResponse> ListLqaProfiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListLqaProfilesQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/lqa/profiles";
        var request =
            new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<LqaProfileDto>(request);

        return new()
        {
            Profiles = response
        };
    }

    [Action("Delete LQA profile", Description = "Delete specific LQA profile")]
    public Task DeleteLqaProfile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] DeleteLqaProfileRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles/{input.LqaProfileUId}", Method.Delete,
            authenticationCredentialsProviders);

        return client.ExecuteWithHandling(request);
    }

    [Action("Get LQA assessment", Description = "Get specific LQA assessment")]
    public async Task<LQAAssessmentDto> GetLQAassessment(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/lqa/assessments/{input.JobUId}", Method.Get,
            authenticationCredentialsProviders);

        return await client.ExecuteWithHandling<LQAAssessmentDto>(request);
    }

    [Action("Run auto LQA", Description = "Runs Auto LQA for specified job parts or all jobs in a given workflow step")]
    public Task RunAutoLQA(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] AutoLQARequest input)
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

        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest(
            $"/api2/v1/projects/{projectRequest.ProjectUId}/runAutoLqa",
            Method.Post, authenticationCredentialsProviders);

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
        return client.ExecuteWithHandling(request);
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