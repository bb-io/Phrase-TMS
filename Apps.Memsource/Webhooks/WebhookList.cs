using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Dtos.Analysis;
using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.OtherHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using RestSharp;
using System.Data;

namespace Apps.PhraseTMS.Webhooks;

[WebhookList("Miscellaneous")]
public class WebhookList(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    #region ProjectWebhooks

    [Webhook("On project created", typeof(ProjectCreationHandler), Description = "On a new project created")]
    public async Task<WebhookResponse<ProjectDto>> ProjectCreation(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectCreatedRequest projectCreatedRequest,
        [WebhookParameter] MultipleSubdomains subdomains,
        [WebhookParameter] MultipleDomains domains)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSProjectCreation] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<ProjectWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSProjectCreation] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (!string.IsNullOrEmpty(projectCreatedRequest.ProjectNameContains) &&
            !data.Project.Name!.Contains(projectCreatedRequest.ProjectNameContains))
        {
            return new()
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (subdomains != null && subdomains.Subdomains != null 
            && !subdomains.Subdomains.Contains(data.Project.SubDomain?.Uid))
        {
            return new()
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (domains != null && domains.Domains != null
            && !domains.Domains.Contains(data.Project.Domain?.Uid))
        {
            return new()
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (!string.IsNullOrEmpty(projectCreatedRequest.CreatedByUsername) &&
        (data.Project.CreatedBy == null ||
         !data.Project.CreatedBy.UserName.Equals(projectCreatedRequest.CreatedByUsername, StringComparison.OrdinalIgnoreCase)))
        {
            return new()
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On project deleted", typeof(ProjectDeletionHandler), Description = "On any project deleted")]
    public async Task<WebhookResponse<ProjectDto>> ProjectDeletion(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSProjectDeletion] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<ProjectWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSProjectDeletion] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (request.ProjectUId != null && data.Project.UId != request.ProjectUId)
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On project due date changed", typeof(ProjectDueDateChangedHandler),
        Description = "On any project due date changed")]
    public async Task<WebhookResponse<ProjectDto>> ProjectDueDateChanged(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSProjectDueDateChanged] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<ProjectWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSProjectDueDateChanged] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (request.ProjectUId != null && data.Project.UId != request.ProjectUId)
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On project metadata updated", typeof(ProjectMetadataUpdatedHandler),
        Description = "On any project metadata updated")]
    public async Task<WebhookResponse<ProjectDto>> ProjectMetadataUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSProjectMetadataUpdated] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<ProjectWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSProjectMetadataUpdated] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (request.ProjectUId != null && data.Project.UId != request.ProjectUId)
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On shared project assigned", typeof(ProjectSharedAssignedHandler),
        Description = "On any shared project assigned")]
    public async Task<WebhookResponse<ProjectDto>> ProjectSharedAssigned(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSProjectSharedAssigned] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<ProjectWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSProjectSharedAssigned] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (request.ProjectUId != null && data.Project.UId != request.ProjectUId)
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On project status changed", typeof(ProjectStatusChangedHandler),
        Description = "On any project status changed")]
    public async Task<WebhookResponse<ProjectDto>> ProjectStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectStatusChangedRequest request,
        [WebhookParameter] ProjectOptionalRequest project,
        [WebhookParameter] [Display("Project name contains")] string? projectNameContains,
        [WebhookParameter] [Display("Project name doesn't contains")] string? projectNameDoesntContains,
        [WebhookParameter] MultipleSubdomains subdomains,
        [WebhookParameter] MultipleDomains domains)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSProjectStatusChanged] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<ProjectWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSProjectStatusChanged] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (request.Status is not null && !request.Status.Contains(data.Project.Status))
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (subdomains != null && subdomains.Subdomains != null
            && !subdomains.Subdomains.Contains(data.Project.SubDomain?.Uid))
        {
            return new()
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (domains != null && domains.Domains != null
            && !domains.Domains.Contains(data.Project.Domain?.Uid))
        {
            return new()
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (project.ProjectUId != null && data.Project.UId != project.ProjectUId)
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (!String.IsNullOrEmpty(projectNameContains) && !data.Project.Name.Contains(projectNameContains))
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (!String.IsNullOrEmpty(projectNameDoesntContains) && data.Project.Name.Contains(projectNameDoesntContains))
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    #endregion

    #region JobWebhooks

    private async Task<MultipleJobResponse> FetchJobs(JobsWrapper? wrapper)
    {
        var jobs = new List<JobDto>();        
        foreach(var part in wrapper?.JobParts ?? [])
        {
            var request = new RestRequest($"/api2/v1/projects/{part.Project.Uid}/jobs/{part.Uid}", Method.Get);
            var job = await Client.ExecuteWithHandling<JobDto>(request);
            jobs.Add(job);
        }
        return new MultipleJobResponse
        {
            Jobs = jobs,
        };
    }

    public class MultipleJobResponse
    {
        public IEnumerable<JobDto> Jobs { get; set; }
    }

    [Webhook("On jobs created", typeof(JobCreationHandler), Description = "Triggered when new jobs are created")]
    public async Task<WebhookResponse<MultipleJobResponse>> JobCreation(WebhookRequest webhookRequest,
        [WebhookParameter] JobCreatedFilters filters)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobCreation] Webhook body is null or empty", []);
            return new WebhookResponse<MultipleJobResponse> { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (data?.JobParts == null || data.JobParts.Count == 0)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobCreation] No job parts found in webhook body. Body: {requestBody}", []);
            return new WebhookResponse<MultipleJobResponse> { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var uniqueProjectUids = data.JobParts
            .Where(p => p?.Project?.Uid != null)
            .Select(p => p.Project.Uid)
            .Distinct()
            .ToList();

        var projectMeta = await LoadProjectsMeta(uniqueProjectUids);

        var shouldTrigger = data.JobParts.Any(p =>
        {
            if (p?.Project?.Uid == null) return false;
            projectMeta.TryGetValue(p.Project.Uid, out var meta);
            return MatchFilters(meta, p, filters);
        });

        if (!shouldTrigger)
            return new WebhookResponse<MultipleJobResponse> { ReceivedWebhookRequestType = WebhookRequestType.Preflight };

        var result = await FetchJobs(data.JobParts);

        return new WebhookResponse<MultipleJobResponse>
        {
            ReceivedWebhookRequestType = WebhookRequestType.Default,
            Result = result
        };
    }

    [Webhook("On jobs deleted", typeof(JobDeletionHandler), Description = "Triggered when any jobs are deleted")]
    public async Task<WebhookResponse<JobsWrapper>> JobDeletion(WebhookRequest webhookRequest)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobDeletion] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobDeletion] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        return new()
        {
            Result = data
        };
    }

    [Webhook("On continuous jobs updated", typeof(JobContinuousUpdatedHandler), Description = "Triggered when continuous jobs are updated")]
    public async Task<WebhookResponse<MultipleJobResponse>> JobContinuousUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobContinuousUpdated] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobContinuousUpdated] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = await FetchJobs(data),
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.All(x => x.Uid != request.JobUId)
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On jobs assigned", typeof(JobAssignedHandler), Description = "Triggered when any jobs are assigned")]
    public async Task<WebhookResponse<MultipleJobResponse>> JobAssigned(WebhookRequest webhookRequest,
        [WebhookParameter] JobAssignedRequest request,
        [WebhookParameter] OptionalJobRequest job)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobAssigned] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobAssigned] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (request.UserId is not null && data.JobParts.FirstOrDefault().assignedTo.All(x => x.Uid != request.UserId))
        {
            return new()
            {
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (job.JobUId != null && data.JobParts.FirstOrDefault()?.Uid != job.JobUId)
        {
            return new()
            {
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = await FetchJobs(data),
        };
    }

    [Webhook("On jobs due date changed", typeof(JobDueDateChangedHandler), Description = "Triggered when the due date of jobs are changed")]
    public async Task<WebhookResponse<MultipleJobResponse>> JobDueDateChanged(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobDueDateChanged] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobDueDateChanged] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = await FetchJobs(data),
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.All(x => x.Uid != request.JobUId)
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On jobs exported", typeof(JobExportedHandler), Description = "Triggered when any jobs are exported")]
    public async Task<WebhookResponse<MultipleJobResponse>> JobExported(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobExported] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobExported] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = await FetchJobs(data),
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.All(x => x.Uid != request.JobUId)
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On jobs source updated", typeof(JobSourceUpdatedHandler), Description = "Triggered when the source file of the jobs are updated")]
    public async Task<WebhookResponse<MultipleJobResponse>> JobSourceUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobSourceUpdated] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobSourceUpdated] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = await FetchJobs(data),
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.All(x => x.Uid != request.JobUId)
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On job status changed", typeof(JobStatusChangedHandler), Description = "On any job status changed")]
    public async Task<WebhookResponse<JobResponse>> JobStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] JobStatusChangedRequest request,
        [WebhookParameter] ProjectOptionalRequest projectOptionalRequest,
        [WebhookParameter] OptionalJobRequest job,
        [WebhookParameter] WorkflowStepOptionalRequest workflowStepRequest,
        [WebhookParameter] MultipleWorkflowStepsOptionalRequest multipleWorkflowSteps,
        [WebhookParameter] OptionalSourceFileIdRequest sourceFileId,
        [WebhookParameter] OptionalSearchJobsQuery jobsQuery,
        [WebhookParameter] [Display("Last workflow level?")] bool? lastWorkflowLevel,
        [WebhookParameter] [Display("Project name contains")] string? projectNameContains,
        [WebhookParameter] [Display("Project name doesn't contains")] string? projectNameDoesntContains,
        [WebhookParameter][Display("Project note contains")] string? projectNoteContains,
        [WebhookParameter][Display("Project note doesn't contain")] string? projectNoteDoesntContains,
        [WebhookParameter] MultipleSubdomains subdomains)
    {
        if (job?.JobUId != null && projectOptionalRequest?.ProjectUId == null)
        {          
            throw new PluginMisconfigurationException("If Job ID is specified in the inputs you must also specify the Project ID");
        }

        if (sourceFileId?.SourceFileId != null && projectOptionalRequest?.ProjectUId == null)
        {
            throw new PluginMisconfigurationException("If Source file ID is specified in the inputs you must also specify the Project ID");
        }

        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobStatusChanged] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobStatusChangedWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobStatusChanged] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (!String.IsNullOrEmpty(projectNameContains) && !data.metadata.project.Name.Contains(projectNameContains))
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (!String.IsNullOrEmpty(projectNameDoesntContains) && data.metadata.project.Name.Contains(projectNameDoesntContains))
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (projectOptionalRequest != null && !String.IsNullOrEmpty(projectOptionalRequest?.ProjectUId) && data.metadata.project.Uid != projectOptionalRequest.ProjectUId)
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (subdomains != null && subdomains.Subdomains != null && !subdomains.Subdomains.Contains(data.metadata.project.subDomain.subDomainUid))
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (!string.IsNullOrWhiteSpace(projectNoteContains) ||
        !string.IsNullOrWhiteSpace(projectNoteDoesntContains))
        {
            var projectIdForNote = data.metadata.project.Uid;
            var note = (await GetProjectNote(projectIdForNote)) ?? string.Empty;

            var pass = true;
            if (!string.IsNullOrWhiteSpace(projectNoteContains))
                pass &= note.IndexOf(projectNoteContains, StringComparison.OrdinalIgnoreCase) >= 0;

            if (!string.IsNullOrWhiteSpace(projectNoteDoesntContains))
                pass &= note.IndexOf(projectNoteDoesntContains, StringComparison.OrdinalIgnoreCase) < 0;

            if (!pass)
            {
                return new()
                {
                    HttpResponseMessage = null,
                    Result = null,
                    ReceivedWebhookRequestType = WebhookRequestType.Preflight
                };
            }
        }

        IEnumerable<JobPart> selectedJobs = data.JobParts;
        var projectId = data.metadata.project.Uid;
        var workflowLevels = new HashSet<int>();

        if (multipleWorkflowSteps?.WorkflowStepIds?.Any() == true)
        {
            foreach (var stepId in multipleWorkflowSteps.WorkflowStepIds.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var level = await Client.GetWorkflowstepLevel(projectId, stepId);
                if (level > 0) workflowLevels.Add(level);
            }
        }

        if (!string.IsNullOrWhiteSpace(workflowStepRequest?.WorkflowStepId))
        {
            var singleLevel = await Client.GetWorkflowstepLevel(projectId, workflowStepRequest.WorkflowStepId);
            if (singleLevel > 0) workflowLevels.Add(singleLevel);
        }

        if (lastWorkflowLevel == true)
        {
            var lastLevel = await Client.GetLastWorkflowstepLevel(projectId);
            if (lastLevel > 0) workflowLevels.Add(lastLevel);
        }

        if (string.IsNullOrEmpty(job?.JobUId) && workflowLevels.Count > 0)
        {
            selectedJobs = selectedJobs.Where(x => workflowLevels.Contains(x.workflowLevel));

            if (!selectedJobs.Any())
            {
                return new()
                {
                    HttpResponseMessage = null,
                    Result = null,
                    ReceivedWebhookRequestType = WebhookRequestType.Preflight
                };
            }
        }

        if (!string.IsNullOrEmpty(job?.JobUId) && !string.IsNullOrEmpty(projectId))
        {
            selectedJobs = data.JobParts.Where(x => x.Uid == job.JobUId);

            if (workflowLevels.Count > 0)
            {
                var single = selectedJobs.FirstOrDefault();
                if (single == null || !workflowLevels.Contains(single.workflowLevel))
                {
                    return new()
                    {
                        HttpResponseMessage = null,
                        Result = null,
                        ReceivedWebhookRequestType = WebhookRequestType.Preflight
                    };
                }
            }
        }
        else if (!string.IsNullOrEmpty(sourceFileId?.SourceFileId) && !string.IsNullOrEmpty(projectId))
        {
            var endpoint = $"/api2/v2/projects/{projectId}/jobs";
            var listJobsRequest = new RestRequest(endpoint.WithQuery(jobsQuery), Method.Get);

            if (workflowLevels.Count == 1)
            {
                listJobsRequest.AddQueryParameter("workflowLevel", workflowLevels.First());
            }

            var listJobsResponse = await Client.Paginate<ListJobDto>(listJobsRequest);
            var filteredByFile = listJobsResponse.Where(x => x.SourceFileUid == sourceFileId.SourceFileId);

            if (workflowLevels.Count > 1)
            {
                filteredByFile = filteredByFile.Where(x =>
                    x.WorkflowStep != null &&
                    workflowLevels.Contains(x.WorkflowStep.WorkflowLevel));
            }

            var filteredJobIds = filteredByFile.Select(x => x.Uid);
            selectedJobs = data.JobParts.IntersectBy(filteredJobIds, x => x.Uid);
        }

        if (jobsQuery.TargetLang != null)
        {
            selectedJobs = selectedJobs.Where(x => x.TargetLang == jobsQuery.TargetLang);
        }

        if (request?.Status != null && request.Status.Any())
        {
            selectedJobs = selectedJobs.Where(x => request.Status.Contains(x.Status));
        }

        var selectedJob = selectedJobs.FirstOrDefault();
        if (selectedJob == null)
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        var projectRequest = new RestRequest($"/api2/v1/projects/{selectedJob.Project.Uid}?with=owners", Method.Get);
        var projectResponse = await Client.ExecuteWithHandling<ProjectDto>(projectRequest);

        var response = new WebhookResponse<JobResponse>
        {
            HttpResponseMessage = null,
            Result = new()
            {
                Uid = selectedJob.Uid,
                Status = selectedJob.Status,
                ProjectUid = selectedJob.Project.Uid,
                Filename = selectedJob.FileName,
                SourceLanguage = projectResponse.SourceLang,
                TargetLanguage = selectedJob.TargetLang,
                ProjectName = data.metadata.project.Name
            }
        };

        return response;
    }

    [Webhook("On all jobs in workflow step reached status", typeof(AllJobsReachedStatusHandler),
        Description =
            "Triggered when all jobs in a specific workflow step reach any of specified statuses. Returns only jobs in the specified workflow step")]
    public async Task<WebhookResponse<ListAllJobsResponse>> HandleAllJobsReachedStatusAsync(WebhookRequest webhookRequest,
        [WebhookParameter] WorkflowStepStatusRequest workflowStepStatusRequest, [WebhookParameter] [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))] string? TargetLang)
    
    {
        var requestBody = webhookRequest.Body?.ToString();
        InvocationContext.Logger?.LogInformation($"[PhraseTMS Webhook] Raw body: {Newtonsoft.Json.JsonConvert.SerializeObject(webhookRequest)}",null);
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMS Webhook] Webhook body is null or empty", null);
            return new WebhookResponse<ListAllJobsResponse> { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var jobData = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (jobData is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMS Webhook] Failed to deserialize webhook body. Body: {requestBody}", null);
            return new WebhookResponse<ListAllJobsResponse> { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var primaryJob = jobData.JobParts?.FirstOrDefault();
        if (primaryJob == null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMS Webhook] No jobs found in webhook body. JobParts count: {jobData.JobParts?.Count ?? 0}", null);
            return new WebhookResponse<ListAllJobsResponse> { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        InvocationContext.Logger?.LogInformation($"[PhraseTMS Webhook] Primary job: uid={primaryJob.Uid}, status={primaryJob.Status}, target={primaryJob.TargetLang}, workflowLevel={primaryJob.workflowLevel}, project={primaryJob.Project?.Uid}",null);

        if (!workflowStepStatusRequest.JobStatuses.Contains(primaryJob.Status))
        {
            return new WebhookResponse<ListAllJobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (primaryJob.Project.Uid != workflowStepStatusRequest.ProjectUId)
        {
            return new WebhookResponse<ListAllJobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (!String.IsNullOrEmpty(TargetLang) && primaryJob.TargetLang != TargetLang) 
        {
            return new WebhookResponse<ListAllJobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        var jobsEndpoint = $"/api2/v2/projects/{workflowStepStatusRequest.ProjectUId}/jobs";
        var apiRequest = new RestRequest(jobsEndpoint, Method.Get);
        apiRequest.AddQueryParameter("workflowLevel", primaryJob.workflowLevel);
        if (!String.IsNullOrEmpty(TargetLang))
        {
            apiRequest.AddQueryParameter("targetLang",TargetLang);
        }

        var allJobs = await Client.Paginate<ListJobDto>(apiRequest);
        if (allJobs.All(job => primaryJob.Uid != job.Uid) || allJobs.Count == 0)
        {
            return new WebhookResponse<ListAllJobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        var relevantJobs = allJobs
            .Where(job => job.WorkflowStep?.Id == workflowStepStatusRequest.WorkflowStepId)
            .Where(job => workflowStepStatusRequest.JobStatuses.Contains(job.Status))
            .ToList();

        InvocationContext.Logger?.LogInformation($"[PhraseTMS Webhook] RelevantJobs.Count={relevantJobs.Count}, JobId;",null);

        if (allJobs.Count != relevantJobs.Count)
        {
            return new WebhookResponse<ListAllJobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
        var returnedIds = string.Join(", ", relevantJobs.Select(j => $"{j.Uid}:{j.Status}"));
        InvocationContext.Logger?.LogInformation($"[PhraseTMS Webhook] Returning {relevantJobs.Count} jobs → [{returnedIds}]",[]);

        return new WebhookResponse<ListAllJobsResponse>
        {
            ReceivedWebhookRequestType = WebhookRequestType.Default,
            Result = new ListAllJobsResponse { Jobs = relevantJobs },
        };
    }

    [Webhook("On job target updated", typeof(JobTargetUpdatedHandler), Description = "Triggered when a job's target has been updated")]
    public async Task<WebhookResponse<JobDto>> JobTargetUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest optionalRequest)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobTargetUpdated] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobTargetUpdated] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var request = new RestRequest($"/api2/v1/projects/{data.JobPart.Project.Uid}/jobs/{data.JobPart.Uid}", Method.Get);
        var job = await Client.ExecuteWithHandling<JobDto>(request);

        return new()
        {
            HttpResponseMessage = null,
            Result = job,
            ReceivedWebhookRequestType = optionalRequest.JobUId != null && data.JobPart.Uid != optionalRequest.JobUId
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On jobs unexported", typeof(JobUnexportedHandler), Description = "Triggered when jobs are exported")]
    public async Task<WebhookResponse<MultipleJobResponse>> JobUnexported(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSJobUnexported] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSJobUnexported] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = await FetchJobs(data),
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.All(x => x.Uid != request.JobUId)
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    private static bool Eq(string? a, string b) =>!string.IsNullOrWhiteSpace(a) && a.Equals(b, StringComparison.OrdinalIgnoreCase);

    private static bool MatchFilters(ProjectDetailsDto? meta, JobPart part, JobCreatedFilters? f)
    {
        if (f == null) return true;

        if (f.Projects?.Any() == true)
        {
            var set = new HashSet<string>(f.Projects.Where(s => !string.IsNullOrWhiteSpace(s))
                                                    .Select(s => s.Trim()),
                                          StringComparer.OrdinalIgnoreCase);
            var projectHit = (meta?.Uid != null && set.Contains(meta.Uid))
                          || (!string.IsNullOrEmpty(meta?.Name) && set.Contains(meta.Name));
            if (!projectHit) return false;
        }

        if (!string.IsNullOrWhiteSpace(f.ProjectOwner))
        {
            var needle = f.ProjectOwner.Trim();
            var o = meta?.Owner;
            var ownerHit = o != null && (Eq(o.Uid, needle) || Eq(o.UserName, needle) || Eq(o.Email, needle));
            if (!ownerHit) return false;
        }

        if (!string.IsNullOrWhiteSpace(f.Domain) && !Eq(meta?.Domain?.Name, f.Domain.Trim()))
            return false;

        if (!string.IsNullOrWhiteSpace(f.SubDomain) && !Eq(meta?.SubDomain?.Name, f.SubDomain.Trim()))
            return false;

        if (!string.IsNullOrWhiteSpace(f.Client))
        {
            var needle = f.Client.Trim();
            var c = meta?.Client;
            var clientHit = c != null && (Eq(c.Uid, needle) || Eq(c.Name, needle));
            if (!clientHit) return false;
        }

        return true;
    }

    private async Task<MultipleJobResponse> FetchJobs(IEnumerable<JobPart> parts)
    {
        var jobs = new List<JobDto>();

        foreach (var part in parts)
        {
            var request = new RestRequest($"/api2/v1/projects/{part.Project.Uid}/jobs/{part.Uid}", Method.Get);
            var job = await Client.ExecuteWithHandling<JobDto>(request);
            jobs.Add(job);
        }

        return new MultipleJobResponse
        {
            Jobs = jobs,
        };
    }

    private async Task<Dictionary<string, ProjectDetailsDto>> LoadProjectsMeta(IEnumerable<string> projectUids)
    {
        var dict = new Dictionary<string, ProjectDetailsDto>(StringComparer.OrdinalIgnoreCase);

        foreach (var uid in projectUids)
        {
            var req = new RestRequest($"/api2/v1/projects/{uid}", Method.Get);
            var meta = await Client.ExecuteWithHandling<ProjectDetailsDto>(req);
            if (meta?.Uid != null)
                dict[meta.Uid] = meta;
        }

        return dict;
    }

    public async Task<string?> GetProjectNote(string projectUid)
    {
        var req = new RestRequest($"/api2/v1/projects/{projectUid}", Method.Get);
        var resp = await Client.ExecuteWithHandling(req);
        if (string.IsNullOrWhiteSpace(resp.Content)) return null;

        try
        {
            var j = Newtonsoft.Json.Linq.JObject.Parse(resp.Content);
            return j["note"]?.ToString();
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region OtherWebhooks

    [Webhook("On analysis created", typeof(AnalysisCreationHandler), Description = "Trigered when a new analysis has been created")]
    public async Task<WebhookResponse<AnalysisDto>> AnalysisCreation(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest projectFilter)
    {
        var requestBody = webhookRequest.Body?.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            InvocationContext.Logger?.LogError("[PhraseTMSAnalysisCreation] Webhook body is null or empty", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        var data = JsonConvert.DeserializeObject<AnalyseWrapper>(requestBody);
        if (data is null)
        {
            InvocationContext.Logger?.LogError($"[PhraseTMSAnalysisCreation] Failed to deserialize webhook body. Body: {requestBody}", []);
            return new() { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        if (projectFilter != null && !String.IsNullOrEmpty(projectFilter.ProjectUId)
            && data.Analyse.Project.ProjectId != projectFilter.ProjectUId)
        {
            return new WebhookResponse<AnalysisDto> { ReceivedWebhookRequestType = WebhookRequestType.Preflight };
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.Analyse
        };
    }

    #endregion
}

public class ProjectWrapper
{
    public ProjectDto Project { get; set; }
}

public class JobsWrapper
{
    [Display("Jobs")]
    public List<JobPart> JobParts { get; set; }
}

public class JobWrapper
{
    public JobPart JobPart { get; set; }
}

public class JobPart
{
    [Display("Job ID")]
    public string Uid { get; set; }
    public Project Project { get; set; }
    public string Status { get; set; }

    [Display("File name")]
    public string FileName { get; set; }

    [Display("Target language code")]
    public string TargetLang { get; set; }

    [Display("Workflow level")]
    public int workflowLevel { get; set; }

    [DefinitionIgnore]
    public List<Provider> assignedTo { get; set; }
}

public class Provider
{
    public string Uid { get; set; }
}

public class AnalyseWrapper
{
    public AnalysisDto Analyse { get; set; }
}

public class JobData
{
    public string Uid { get; set; }
    public string ServerTaskId { get; set; }
    public int LastWorkflowLevel { get; set; }

    [JsonProperty("project")] public TaskProject Project { get; set; }
}

public class TaskJob
{
    public string uid { get; set; }
}

public class TaskProject
{
    public string Uid { get; set; }
}

public class TaskData
{
    public string workflowLevel { get; set; }
    public string taskId { get; set; }

    [JsonProperty("Job")] public TaskJob job { get; set; }

    [JsonProperty("Project")] public TaskProject project { get; set; }
    public string resourcePath { get; set; }
}

public class JobStatusChangedWrapper
{
    public List<JobPart> JobParts { get; set; }
    [JsonProperty("metadata")] public ProjectMetadata metadata { get; set; }
}

public class ProjectMetadata
{
    public Project project { get; set; }
}

public class Project
{
    [Display("Project ID")]
    public string Uid { get; set; }

    [Display("Last workflow level")]
    public int LastWorkflowLevel { get; set; }

    [Display("Name")]
    public string Name { get; set; }

    public subdomain subDomain { get; set; }
}

public class subdomain
{
    [Display("Subdomain name")]
    [JsonProperty("name")] public string subDomainName { get; set; }

    [Display("Subdomain Uid")]
    [JsonProperty("Uid")] public string subDomainUid { get; set; }
}