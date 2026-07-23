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
using Apps.PhraseTMS.Webhooks.Models;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Apps.PhraseTMS.Webhooks.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text;
using System.Globalization;

namespace Apps.PhraseTMS.Webhooks;

[WebhookList("Miscellaneous")]
public class WebhookList(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    private const int WebhookLogArgumentMaxLength = 4000;

    #region ProjectWebhooks

    [Webhook("On project created", typeof(ProjectCreationHandler), Description = "Triggered when a project is created")]
    public Task<WebhookResponse<ProjectDto>> ProjectCreation(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectCreatedRequest projectCreatedRequest,
        [WebhookParameter] MultipleSubdomains subdomains,
        [WebhookParameter] MultipleDomains domains)
        => ExecuteWebhookSafelyAsync("PhraseTMSProjectCreation", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<ProjectWrapper, ProjectDto>(webhookRequest, "PhraseTMSProjectCreation",
                    out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            var projectNameContains = projectCreatedRequest.ProjectNameContains?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

            if (projectNameContains is { Length: > 0 } &&
                !projectNameContains.Any(x => data.Project.Name!.Contains(x)))
            {
                return Preflight<ProjectDto>();
            }

            if (subdomains?.Subdomains != null && !subdomains.Subdomains.Contains(data.Project.SubDomain?.Uid))
            {
                return Preflight<ProjectDto>();
            }

            if (domains?.Domains != null && !domains.Domains.Contains(data.Project.Domain?.Uid))
            {
                return Preflight<ProjectDto>();
            }

            if (!string.IsNullOrEmpty(projectCreatedRequest.CreatedByUsername) &&
                (data.Project.CreatedBy == null ||
                 !data.Project.CreatedBy.UserName.Equals(projectCreatedRequest.CreatedByUsername,
                     StringComparison.OrdinalIgnoreCase)))
            {
                return Preflight<ProjectDto>();
            }

            return Success(data.Project);
        });

    [Webhook("On project deleted", typeof(ProjectDeletionHandler), Description = "Triggered when a project is deleted")]
    public Task<WebhookResponse<ProjectDto>> ProjectDeletion(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSProjectDeletion", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<ProjectWrapper, ProjectDto>(webhookRequest, "PhraseTMSProjectDeletion",
                    out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (request.ProjectUId != null && data.Project.UId != request.ProjectUId)
            {
                return Preflight<ProjectDto>();
            }

            return Success(data.Project);
        });

    [Webhook("On project due date changed", typeof(ProjectDueDateChangedHandler),
        Description = "Triggered when a project due date changes")]
    public Task<WebhookResponse<ProjectDto>> ProjectDueDateChanged(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSProjectDueDateChanged", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<ProjectWrapper, ProjectDto>(webhookRequest,
                    "PhraseTMSProjectDueDateChanged", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (request.ProjectUId != null && data.Project.UId != request.ProjectUId)
            {
                return Preflight<ProjectDto>();
            }

            return Success(data.Project);
        });

    [Webhook("On project metadata updated", typeof(ProjectMetadataUpdatedHandler),
        Description = "Triggered when project metadata is updated")]
    public Task<WebhookResponse<ProjectDto>> ProjectMetadataUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSProjectMetadataUpdated", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<ProjectWrapper, ProjectDto>(webhookRequest,
                    "PhraseTMSProjectMetadataUpdated", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (request.ProjectUId != null && data.Project.UId != request.ProjectUId)
            {
                return Preflight<ProjectDto>();
            }

            return Success(data.Project);
        });

    [Webhook("On shared project assigned", typeof(ProjectSharedAssignedHandler),
        Description = "Triggered when a shared project is assigned")]
    public Task<WebhookResponse<ProjectDto>> ProjectSharedAssigned(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest request,
        [WebhookParameter] [Display("Domain name")] string? Domain,
        [WebhookParameter] MultipleDomains domains)
        => ExecuteWebhookSafelyAsync("PhraseTMSProjectSharedAssigned", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<ProjectWrapper, ProjectDto>(webhookRequest,
                    "PhraseTMSProjectSharedAssigned", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (request.ProjectUId != null && data.Project.UId != request.ProjectUId)
            {
                return Preflight<ProjectDto>();
            }

            if (domains?.Domains != null && !domains.Domains.Contains(data.Project.Domain?.Uid))
            {
                return Preflight<ProjectDto>();
            }

            if (!string.IsNullOrEmpty(Domain) && Domain != data.Project.Domain?.Name)
            {
                return Preflight<ProjectDto>();
            }

            return Success(data.Project);
        });

    [Webhook("On project status changed", typeof(ProjectStatusChangedHandler),
        Description = "Triggered when a project status changes")]
    public Task<WebhookResponse<ProjectDto>> ProjectStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectStatusChangedRequest request,
        [WebhookParameter] ProjectOptionalRequest project,
        [WebhookParameter] [Display("Project name contains")] string? projectNameContains,
        [WebhookParameter] [Display("Project name doesn't contains")] string? projectNameDoesntContains,
        [WebhookParameter] MultipleSubdomains subdomains,
        [WebhookParameter] MultipleDomains domains)
        => ExecuteWebhookSafelyAsync("PhraseTMSProjectStatusChanged", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<ProjectWrapper, ProjectDto>(webhookRequest,
                    "PhraseTMSProjectStatusChanged", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (request.Status is not null && !request.Status.Contains(data.Project.Status))
            {
                return Preflight<ProjectDto>();
            }

            if (subdomains?.Subdomains != null && !subdomains.Subdomains.Contains(data.Project.SubDomain?.Uid))
            {
                return Preflight<ProjectDto>();
            }

            if (domains?.Domains != null && !domains.Domains.Contains(data.Project.Domain?.Uid))
            {
                return Preflight<ProjectDto>();
            }

            if (project.ProjectUId != null && data.Project.UId != project.ProjectUId)
            {
                return Preflight<ProjectDto>();
            }

            if (!string.IsNullOrEmpty(projectNameContains) && !data.Project.Name.Contains(projectNameContains))
            {
                return Preflight<ProjectDto>();
            }

            if (!string.IsNullOrEmpty(projectNameDoesntContains) &&
                data.Project.Name.Contains(projectNameDoesntContains))
            {
                return Preflight<ProjectDto>();
            }

            return Success(data.Project);
        });

    #endregion

    #region JobWebhooks

    [Webhook("On jobs created", typeof(JobCreationHandler), Description = "Triggered when new jobs are created")]
    public Task<WebhookResponse<MultipleJobResponse>> JobCreation(WebhookRequest webhookRequest,
        [WebhookParameter] JobCreatedFilters filters,
        [WebhookParameter] WorkflowStepOptionalRequest workflowStepRequest)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobCreation", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, MultipleJobResponse>(webhookRequest, "PhraseTMSJobCreation",
                    out var requestBody, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (data.JobParts == null || data.JobParts.Count == 0)
            {
                LogWebhook("PhraseTMSJobCreation", LogLevel.Error,
                    "No job parts found in webhook body. Body: {0}",
                    requestBody);
                return Preflight<MultipleJobResponse>();
            }

            var uniqueProjectUids = data.JobParts
                .Where(p => p?.Project?.Uid != null)
                .Select(p => p.Project.Uid)
                .Distinct()
                .ToList();

            var projectMeta = await LoadProjectsMeta(uniqueProjectUids);
            var workflowLevelsByProject = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            if (!string.IsNullOrWhiteSpace(workflowStepRequest.WorkflowStepId))
            {
                foreach (var projectUid in uniqueProjectUids)
                {
                    var workflowLevel =
                        await Client.GetWorkflowstepLevel(projectUid, workflowStepRequest.WorkflowStepId, false);
                    if (workflowLevel > 0)
                    {
                        workflowLevelsByProject[projectUid] = workflowLevel;
                    }
                }
            }

            var shouldTrigger = data.JobParts.Any(p =>
            {
                if (p?.Project?.Uid == null)
                {
                    return false;
                }

                projectMeta.TryGetValue(p.Project.Uid, out var meta);
                if (!MatchFilters(meta, p, filters))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(workflowStepRequest.WorkflowStepId))
                {
                    return true;
                }

                return workflowLevelsByProject.TryGetValue(p.Project.Uid, out var workflowLevel)
                    && workflowLevel == p.workflowLevel;
            });

            if (!shouldTrigger)
            {
                return Preflight<MultipleJobResponse>();
            }

            return Success(await FetchJobs(data.JobParts));
        });

    [Webhook("On jobs deleted", typeof(JobDeletionHandler), Description = "Triggered when any jobs are deleted")]
    public Task<WebhookResponse<JobsWrapper>> JobDeletion(WebhookRequest webhookRequest)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobDeletion", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, JobsWrapper>(webhookRequest, "PhraseTMSJobDeletion",
                    out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            return Success(data);
        });

    [Webhook("On continuous jobs updated", typeof(JobContinuousUpdatedHandler), Description = "Triggered when continuous jobs are updated")]
    public Task<WebhookResponse<MultipleJobResponse>> JobContinuousUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobContinuousUpdated", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, MultipleJobResponse>(webhookRequest,
                    "PhraseTMSJobContinuousUpdated", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            return Success(await FetchJobs(data.JobParts), MatchByJobUid(data.JobParts, request.JobUId));
        });

    [Webhook("On jobs assigned", typeof(JobAssignedHandler), Description = "Triggered when any jobs are assigned")]
    public Task<WebhookResponse<MultipleJobResponse>> JobAssigned(WebhookRequest webhookRequest,
        [WebhookParameter] JobAssignedRequest request,
        [WebhookParameter] OptionalJobRequest job)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobAssigned", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, MultipleJobResponse>(webhookRequest, "PhraseTMSJobAssigned",
                    out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            var firstJob = data.JobParts.FirstOrDefault();

            if (request.UserId is not null && firstJob?.assignedTo.All(x => x.Uid != request.UserId) == true)
            {
                return Preflight<MultipleJobResponse>();
            }

            if (job.JobUId != null && firstJob?.Uid != job.JobUId)
            {
                return Preflight<MultipleJobResponse>();
            }

            return Success(await FetchJobs(data.JobParts));
        });

    [Webhook("On jobs due date changed", typeof(JobDueDateChangedHandler), Description = "Triggered when job due dates change")]
    public Task<WebhookResponse<MultipleJobResponse>> JobDueDateChanged(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobDueDateChanged", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, MultipleJobResponse>(webhookRequest,
                    "PhraseTMSJobDueDateChanged", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            return Success(await FetchJobs(data.JobParts), MatchByJobUid(data.JobParts, request.JobUId));
        });

    [Webhook("On jobs exported", typeof(JobExportedHandler), Description = "Triggered when any jobs are exported")]
    public Task<WebhookResponse<MultipleJobResponse>> JobExported(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobExported", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, MultipleJobResponse>(webhookRequest,
                    "PhraseTMSJobExported", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            return Success(await FetchJobs(data.JobParts), MatchByJobUid(data.JobParts, request.JobUId));
        });

    [Webhook("On jobs source updated", typeof(JobSourceUpdatedHandler), Description = "Triggered when job source files are updated")]
    public Task<WebhookResponse<MultipleJobResponse>> JobSourceUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobSourceUpdated", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, MultipleJobResponse>(webhookRequest,
                    "PhraseTMSJobSourceUpdated", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            return Success(await FetchJobs(data.JobParts), MatchByJobUid(data.JobParts, request.JobUId));
        });

    [Webhook("On job custom field updated", typeof(JobCustomFieldsUpdatedHandler),
        Description = "Triggered when job custom fields are updated")]
    public Task<WebhookResponse<JobCustomFieldsUpdatedResponse>> JobCustomFieldsUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest projectRequest,
        [WebhookParameter] OptionalJobRequest jobRequest,
        [WebhookParameter] OptionalJobCustomFieldRequest customFieldRequest)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobCustomFieldsUpdated", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<JobCustomFieldsUpdatedPayload, JobCustomFieldsUpdatedResponse>(
                    webhookRequest, "PhraseTMSJobCustomFieldsUpdated", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (data.JobParts == null || data.JobParts.Count == 0)
            {
                return Preflight<JobCustomFieldsUpdatedResponse>();
            }

            var eventTimestamp = DateTimeOffset.FromUnixTimeSeconds(data.Timestamp).ToUniversalTime();

            var updatedCustomFields = data.JobParts
                .Where(x => string.IsNullOrWhiteSpace(projectRequest.ProjectUId) || x.Project?.Uid == projectRequest.ProjectUId)
                .Where(x => string.IsNullOrWhiteSpace(jobRequest.JobUId) || x.UId == jobRequest.JobUId)
                .SelectMany(x => GetUpdatedCustomFields(x.CustomFields, eventTimestamp)
                    .Where(f => string.IsNullOrWhiteSpace(customFieldRequest.CustomFieldUId)
                        || f.CustomField?.UId == customFieldRequest.CustomFieldUId)
                    .Select(f => MapUpdatedCustomField(
                        f,
                        x.UId,
                        x.Project?.Uid ?? data.Metadata?.project?.Uid,
                        data.Metadata?.project?.Name,
                        x.FileName,
                        x.TargetLang,
                        x.WorkflowLevel)))
                .ToList();

            if (updatedCustomFields.Count == 0)
            {
                return Preflight<JobCustomFieldsUpdatedResponse>();
            }

            return Success(new JobCustomFieldsUpdatedResponse
            {
                EventUId = data.EventUId,
                EventTimestamp = FormatDateTimeOffset(eventTimestamp),
                UpdatedCustomFields = updatedCustomFields
            });
        });

    [Webhook("On pre-translation finished", typeof(PreTranslationFinishedHandler),
        Description = "Triggered when pre-translation finishes for a job")]
    public Task<WebhookResponse<JobResponse>> PreTranslationFinished(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest projectOptionalRequest,
        [WebhookParameter] PreTranslationFinishedRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSPreTranslationFinished", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, JobResponse>(webhookRequest,
                    "PhraseTMSPreTranslationFinished", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            var matchingJob = data.JobParts?.FirstOrDefault(x => x.Uid == request.JobUId);
            if (matchingJob == null || string.IsNullOrWhiteSpace(matchingJob.Project?.Uid))
            {
                return Preflight<JobResponse>();
            }

            if (!string.IsNullOrWhiteSpace(projectOptionalRequest.ProjectUId) &&
                !string.Equals(projectOptionalRequest.ProjectUId, matchingJob.Project.Uid,
                    StringComparison.OrdinalIgnoreCase))
            {
                return Preflight<JobResponse>();
            }

            var jobRequest = new RestRequest($"/api2/v1/projects/{matchingJob.Project.Uid}/jobs/{matchingJob.Uid}",
                Method.Get);
            var job = await Client.ExecuteWithHandling<JobDto>(jobRequest);

            return Success(MapJobResponse(job));
        });

    [Webhook("On job status changed", typeof(JobStatusChangedHandler), Description = "Triggered when a job status changes")]
    public Task<WebhookResponse<JobResponse>> JobStatusChanged(WebhookRequest webhookRequest,
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
        [WebhookParameter] [Display("Project note contains")] string? projectNoteContains,
        [WebhookParameter] [Display("Project note doesn't contain")] string? projectNoteDoesntContains,
        [WebhookParameter] MultipleSubdomains subdomains)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobStatusChanged", webhookRequest, async () =>
        {
            if (job?.JobUId != null && projectOptionalRequest?.ProjectUId == null)
            {
                throw new PluginMisconfigurationException(
                    "If Job ID is specified in the inputs you must also specify the Project ID");
            }

            if (sourceFileId?.SourceFileId != null && projectOptionalRequest?.ProjectUId == null)
            {
                throw new PluginMisconfigurationException(
                    "If Source file ID is specified in the inputs you must also specify the Project ID");
            }

            if (!TryDeserializeWebhookPayload<JobStatusChangedWrapper, JobResponse>(webhookRequest,
                    "PhraseTMSJobStatusChanged", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (!string.IsNullOrEmpty(projectNameContains) &&
                !data.metadata.project.Name.Contains(projectNameContains))
            {
                return Preflight<JobResponse>();
            }

            if (!string.IsNullOrEmpty(projectNameDoesntContains) &&
                data.metadata.project.Name.Contains(projectNameDoesntContains))
            {
                return Preflight<JobResponse>();
            }

            if (projectOptionalRequest != null && !string.IsNullOrEmpty(projectOptionalRequest.ProjectUId) &&
                data.metadata.project.Uid != projectOptionalRequest.ProjectUId)
            {
                return Preflight<JobResponse>();
            }

            if (subdomains?.Subdomains != null &&
                !subdomains.Subdomains.Contains(data.metadata.project.subDomain.subDomainUid))
            {
                return Preflight<JobResponse>();
            }

            if (!string.IsNullOrWhiteSpace(projectNoteContains) || !string.IsNullOrWhiteSpace(projectNoteDoesntContains))
            {
                var projectIdForNote = data.metadata.project.Uid;
                var note = (await GetProjectNote(projectIdForNote)) ?? string.Empty;

                var pass = true;
                if (!string.IsNullOrWhiteSpace(projectNoteContains))
                {
                    pass &= note.IndexOf(projectNoteContains, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                if (!string.IsNullOrWhiteSpace(projectNoteDoesntContains))
                {
                    pass &= note.IndexOf(projectNoteDoesntContains, StringComparison.OrdinalIgnoreCase) < 0;
                }

                if (!pass)
                {
                    return Preflight<JobResponse>();
                }
            }

            IEnumerable<JobPart> selectedJobs = data.JobParts;
            var projectId = data.metadata.project.Uid;

            var hasWorkflowStepFilter =
                multipleWorkflowSteps?.WorkflowStepIds?.Any() == true ||
                !string.IsNullOrWhiteSpace(workflowStepRequest?.WorkflowStepId) ||
                lastWorkflowLevel == true;
            var workflowLevels = new HashSet<int>();

            if (multipleWorkflowSteps?.WorkflowStepIds?.Any() == true)
            {
                foreach (var stepId in multipleWorkflowSteps.WorkflowStepIds.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    var level = await Client.GetWorkflowstepLevel(projectId, stepId, false);
                    if (level > 0)
                    {
                        workflowLevels.Add(level);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(workflowStepRequest?.WorkflowStepId))
            {
                var singleLevel = await Client.GetWorkflowstepLevel(projectId, workflowStepRequest.WorkflowStepId, false);
                if (singleLevel > 0)
                {
                    workflowLevels.Add(singleLevel);
                }
            }

            if (lastWorkflowLevel == true)
            {
                var lastLevel = await Client.GetLastWorkflowstepLevel(projectId);
                if (lastLevel > 0)
                {
                    workflowLevels.Add(lastLevel);
                }
            }

            if (string.IsNullOrEmpty(job?.JobUId) && hasWorkflowStepFilter)
            {
                if (workflowLevels.Count == 0)
                {
                    return Preflight<JobResponse>();
                }

                selectedJobs = selectedJobs.Where(x => workflowLevels.Contains(x.workflowLevel));

                if (!selectedJobs.Any())
                {
                    return Preflight<JobResponse>();
                }
            }

            if (!string.IsNullOrEmpty(job?.JobUId) && !string.IsNullOrEmpty(projectId))
            {
                selectedJobs = data.JobParts.Where(x => x.Uid == job.JobUId);

                if (hasWorkflowStepFilter)
                {
                    var single = selectedJobs.FirstOrDefault();
                    if (single == null || !workflowLevels.Contains(single.workflowLevel))
                    {
                        return Preflight<JobResponse>();
                    }
                }
            }
            else if (!string.IsNullOrEmpty(sourceFileId?.SourceFileId) && !string.IsNullOrEmpty(projectId))
            {
                if (hasWorkflowStepFilter && workflowLevels.Count == 0)
                {
                    return Preflight<JobResponse>();
                }

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
                return Preflight<JobResponse>();
            }

            var projectRequest = new RestRequest($"/api2/v1/projects/{selectedJob.Project.Uid}?with=owners", Method.Get);
            var projectResponse = await Client.ExecuteWithHandling<ProjectDto>(projectRequest);

            return Success(new JobResponse
            {
                Uid = selectedJob.Uid,
                Status = selectedJob.Status,
                ProjectUid = selectedJob.Project.Uid,
                Filename = selectedJob.FileName,
                SourceLanguage = projectResponse.SourceLang,
                TargetLanguage = selectedJob.TargetLang,
                ProjectName = data.metadata.project.Name
            });
        });

    [Webhook("On all jobs in workflow step reached status", typeof(AllJobsReachedStatusHandler),
        Description =
            "Triggered when all jobs in a specific workflow step reach any of the specified statuses. Outputs only jobs in that workflow step")]
    public Task<WebhookResponse<ListAllJobsResponse>> HandleAllJobsReachedStatusAsync(WebhookRequest webhookRequest,
        [WebhookParameter] WorkflowStepStatusRequest workflowStepStatusRequest, [WebhookParameter] [Display("Target language")]
        [DataSource(typeof(LanguageDataHandler))] string? TargetLang)
        => ExecuteWebhookSafelyAsync("PhraseTMSAllJobsReachedStatus", webhookRequest, async () =>
        {
            LogWebhook("PhraseTMS Webhook", LogLevel.Information,
                "Raw body: {0}",
                JsonConvert.SerializeObject(webhookRequest));

            if (!TryDeserializeWebhookPayload<JobsWrapper, ListAllJobsResponse>(webhookRequest, "PhraseTMS Webhook",
                    out _, out var jobData, out var errorResponse))
            {
                return errorResponse!;
            }

            var primaryJob = jobData.JobParts?.FirstOrDefault();
            if (primaryJob == null)
            {
                LogWebhook("PhraseTMS Webhook", LogLevel.Error,
                    "No jobs found in webhook body. JobParts count: {0}",
                    jobData.JobParts?.Count ?? 0);
                return Preflight<ListAllJobsResponse>();
            }

            LogWebhook("PhraseTMS Webhook", LogLevel.Information,
                "Primary job: uid={0}, status={1}, target={2}, workflowLevel={3}, project={4}",
                primaryJob.Uid,
                primaryJob.Status,
                primaryJob.TargetLang,
                primaryJob.workflowLevel,
                primaryJob.Project?.Uid);

            if (!workflowStepStatusRequest.JobStatuses.Contains(primaryJob.Status))
            {
                return Preflight<ListAllJobsResponse>();
            }

            if (workflowStepStatusRequest.ProjectUId != null &&
                primaryJob.Project.Uid != workflowStepStatusRequest.ProjectUId)
            {
                return Preflight<ListAllJobsResponse>();
            }

            if (!string.IsNullOrEmpty(TargetLang) && primaryJob.TargetLang != TargetLang)
            {
                return Preflight<ListAllJobsResponse>();
            }

            var jobsEndpoint = $"/api2/v2/projects/{workflowStepStatusRequest.ProjectUId ?? primaryJob.Project?.Uid}/jobs";
            var apiRequest = new RestRequest(jobsEndpoint, Method.Get);
            apiRequest.AddQueryParameter("workflowLevel", primaryJob.workflowLevel);
            if (!string.IsNullOrEmpty(TargetLang))
            {
                apiRequest.AddQueryParameter("targetLang", TargetLang);
            }

            var allJobs = await Client.Paginate<ListJobDto>(apiRequest);
            if (allJobs.All(job => primaryJob.Uid != job.Uid) || allJobs.Count == 0)
            {
                return Preflight<ListAllJobsResponse>();
            }

            var relevantJobs = allJobs
                .Where(job =>
                    job.WorkflowStep?.Id == workflowStepStatusRequest.WorkflowStepId ||
                    job.WorkflowStep?.WorkflowLevel == workflowStepStatusRequest.WorkflowLevel)
                .Where(job => workflowStepStatusRequest.JobStatuses.Contains(job.Status))
                .ToList();

            LogWebhook("PhraseTMS Webhook", LogLevel.Information,
                "RelevantJobs.Count={0}",
                relevantJobs.Count);

            if (allJobs.Count != relevantJobs.Count)
            {
                return Preflight<ListAllJobsResponse>();
            }

            var returnedIds = string.Join(", ", relevantJobs.Select(j => $"{j.Uid}:{j.Status}"));
            LogWebhook("PhraseTMS Webhook", LogLevel.Information,
                "Returning {0} jobs → [{1}]",
                relevantJobs.Count,
                returnedIds);

            return Success(new ListAllJobsResponse { Jobs = relevantJobs });
        });

    [Webhook("On job target updated", typeof(JobTargetUpdatedHandler), Description = "Triggered when a job's target has been updated")]
    public Task<WebhookResponse<JobDto>> JobTargetUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest optionalRequest)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobTargetUpdated", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobWrapper, JobDto>(webhookRequest, "PhraseTMSJobTargetUpdated",
                    out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            var request = new RestRequest($"/api2/v1/projects/{data.JobPart.Project.Uid}/jobs/{data.JobPart.Uid}", Method.Get);
            var job = await Client.ExecuteWithHandling<JobDto>(request);

            return Success(job, optionalRequest.JobUId != null && data.JobPart.Uid != optionalRequest.JobUId
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default);
        });

    [Webhook("On jobs unexported", typeof(JobUnexportedHandler), Description = "Triggered when jobs are unexported")]
    public Task<WebhookResponse<MultipleJobResponse>> JobUnexported(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
        => ExecuteWebhookSafelyAsync("PhraseTMSJobUnexported", webhookRequest, async () =>
        {
            if (!TryDeserializeWebhookPayload<JobsWrapper, MultipleJobResponse>(webhookRequest,
                    "PhraseTMSJobUnexported", out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            return Success(await FetchJobs(data.JobParts), MatchByJobUid(data.JobParts, request.JobUId));
        });
    
    #endregion

    #region OtherWebhooks

    [Webhook("On analysis created", typeof(AnalysisCreationHandler), Description = "Triggered when a new analysis is created")]
    public Task<WebhookResponse<AnalysisDto>> AnalysisCreation(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectOptionalRequest projectFilter)
        => ExecuteWebhookSafelyAsync("PhraseTMSAnalysisCreation", webhookRequest, () =>
        {
            if (!TryDeserializeWebhookPayload<AnalyseWrapper, AnalysisDto>(webhookRequest, "PhraseTMSAnalysisCreation",
                    out _, out var data, out var errorResponse))
            {
                return errorResponse!;
            }

            if (projectFilter != null && !string.IsNullOrEmpty(projectFilter.ProjectUId) &&
                data.Analyse.Project.ProjectId != projectFilter.ProjectUId)
            {
                return Preflight<AnalysisDto>();
            }

            return Success(data.Analyse);
        });

    #endregion

    #region Helpers

    private static WebhookResponse<T> Success<T>(T? result,
        WebhookRequestType requestType = WebhookRequestType.Default) where T : class
        => new()
        {
            HttpResponseMessage = null,
            Result = result,
            ReceivedWebhookRequestType = requestType
        };

    private static WebhookResponse<T> Preflight<T>() where T : class
        => Success<T>(null, WebhookRequestType.Preflight);

    private static IEnumerable<JobCustomFieldPayload> GetUpdatedCustomFields(
        IEnumerable<JobCustomFieldPayload>? customFields, DateTimeOffset eventTimestamp)
    {
        if (customFields == null)
        {
            return [];
        }

        var eventTimestampTruncated = TruncateToSeconds(eventTimestamp);

        return customFields.Where(x =>
        {
            var effectiveTime = x.UpdatedAt ?? x.CreatedAt;
            return effectiveTime.HasValue && TruncateToSeconds(effectiveTime.Value) == eventTimestampTruncated;
        });
    }

    private static JobCustomFieldWebhookResponse MapUpdatedCustomField(JobCustomFieldPayload field,
        string jobUId, string? projectUId, string? projectName, string? fileName, string? targetLang,
        int workflowLevel)
        => new()
        {
            JobUId = jobUId,
            ProjectUId = projectUId,
            ProjectName = projectName,
            FileName = fileName,
            TargetLang = targetLang,
            WorkflowLevel = workflowLevel,
            UId = field.UId,
            CustomFieldUId = field.CustomField?.UId,
            Name = field.CustomField?.Name,
            Type = field.CustomField?.Type,
            Value = field.Value,
            CreatedAt = FormatNullableDateTimeOffset(field.CreatedAt),
            UpdatedAt = FormatNullableDateTimeOffset(field.UpdatedAt),
            SelectedOptions = field.SelectedOptions.Select(x => new JobCustomFieldOptionWebhookResponse
            {
                UId = x.UId,
                Value = x.Value
            })
        };

    private static DateTimeOffset TruncateToSeconds(DateTimeOffset value)
        => value.ToUniversalTime().AddTicks(-(value.Ticks % TimeSpan.TicksPerSecond));

    private static string FormatDateTimeOffset(DateTimeOffset value)
        => value.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture);

    private static string? FormatNullableDateTimeOffset(DateTimeOffset? value)
        => value.HasValue ? FormatDateTimeOffset(value.Value) : null;

    private static WebhookRequestType MatchByJobUid(IEnumerable<JobPart> jobParts, string? jobUid)
        => jobUid != null && jobParts.All(x => x.Uid != jobUid)
            ? WebhookRequestType.Preflight
            : WebhookRequestType.Default;

    private static JobResponse MapJobResponse(JobDto job)
        => new()
        {
            Uid = job.Uid,
            ProjectUid = job.Project?.UId ?? string.Empty,
            ProjectName = job.Project?.Name ?? string.Empty,
            Filename = job.Filename,
            Status = job.Status,
            TargetLanguage = job.TargetLang,
            SourceLanguage = job.SourceLang,
            WordCount = job.WordsCount
        };

    private static bool Eq(string? a, string b)
        => !string.IsNullOrWhiteSpace(a) && a.Equals(b, StringComparison.OrdinalIgnoreCase);

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

    private Task<WebhookResponse<T>> ExecuteWebhookSafelyAsync<T>(string webhookName, WebhookRequest webhookRequest,
        Func<WebhookResponse<T>> action) where T : class
    {
        try
        {
            return Task.FromResult(action());
        }
        catch (Exception ex)
        {
            return Task.FromResult(CreateBadRequestResponse<T>(webhookName, webhookRequest, ex));
        }
    }

    private async Task<WebhookResponse<T>> ExecuteWebhookSafelyAsync<T>(string webhookName, WebhookRequest webhookRequest,
        Func<Task<WebhookResponse<T>>> action) where T : class
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return CreateBadRequestResponse<T>(webhookName, webhookRequest, ex);
        }
    }

    private bool TryDeserializeWebhookPayload<TPayload, TResponse>(WebhookRequest webhookRequest, string webhookName,
        out string requestBody, out TPayload? payload, out WebhookResponse<TResponse>? errorResponse)
        where TPayload : class
        where TResponse : class
    {
        requestBody = webhookRequest.Body?.ToString() ?? string.Empty;
        payload = default;
        errorResponse = default;

        if (string.IsNullOrWhiteSpace(requestBody))
        {
            LogWebhook(webhookName, LogLevel.Error,
                "Webhook body is null or empty");
            errorResponse = Preflight<TResponse>();
            return false;
        }

        payload = JsonConvert.DeserializeObject<TPayload>(requestBody);
        if (payload is not null)
        {
            return true;
        }

        LogWebhook(webhookName, LogLevel.Error,
            "Failed to deserialize webhook body. Body: {0}",
            requestBody);
        errorResponse = Preflight<TResponse>();
        return false;
    }

    private WebhookResponse<T> CreateBadRequestResponse<T>(string webhookName, WebhookRequest webhookRequest,
        Exception exception) where T : class
    {
        var requestBody = webhookRequest.Body?.ToString();
        var diagnosticPayload = new
        {
            webhook = webhookName,
            error = "Phrase TMS webhook processing failed.",
            exceptionType = exception.GetType().FullName,
            message = exception.Message,
            stackTrace = exception.StackTrace,
            innerExceptionMessage = exception.InnerException?.Message,
            request = new
            {
                method = webhookRequest.HttpMethod?.Method,
                body = Utils.StringExtensions.Truncate(requestBody, 8000)
            }
        };

        var diagnosticJson = JsonConvert.SerializeObject(diagnosticPayload, Formatting.Indented);
        LogWebhook(webhookName, LogLevel.Error,
            "Webhook processing failed. Request method: {0}; Request body: {1}; Exception type: {2}; Exception message: {3}; Stack trace: {4}",
            webhookRequest.HttpMethod?.Method,
            requestBody,
            exception.GetType().FullName,
            exception.Message,
            exception.StackTrace);

        var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(diagnosticJson, Encoding.UTF8, "application/json")
        };

        return new WebhookResponse<T>
        {
            HttpResponseMessage = httpResponse,
            Result = null,
            ReceivedWebhookRequestType = WebhookRequestType.Preflight
        };
    }

    private void LogWebhook(string webhookName, LogLevel logLevel, string messageTemplate, params object?[] args)
    {
        var formattedArgs = args
            .Select(x =>
            {
                var formattedValue = x switch
                {
                    null => string.Empty,
                    IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture) ?? string.Empty,
                    _ => x.ToString() ?? string.Empty
                };

                return Utils.StringExtensions.Truncate(formattedValue, WebhookLogArgumentMaxLength) ?? string.Empty;
            })
            .Cast<object>()
            .ToArray();

        string message;
        try
        {
            message = formattedArgs.Length == 0
                ? messageTemplate
                : string.Format(CultureInfo.InvariantCulture, messageTemplate, formattedArgs);
        }
        catch (FormatException)
        {
            var fallbackArgs = string.Join("; ", formattedArgs.Select((x, i) => $"arg{i}: {x}"));
            message = $"{messageTemplate}; {fallbackArgs}";
        }

        message = $"[{webhookName}] {message}".Replace("{", "{{").Replace("}", "}}");

        var logAction = logLevel switch
        {
            LogLevel.Critical => InvocationContext.Logger?.LogCritical,
            LogLevel.Error => InvocationContext.Logger?.LogError,
            LogLevel.Warning => InvocationContext.Logger?.LogWarning,
            LogLevel.Debug or LogLevel.Trace => InvocationContext.Logger?.LogDebug,
            LogLevel.None => null,
            _ => InvocationContext.Logger?.LogInformation
        };

        logAction?.Invoke(message, []);
    }

    private async Task<string?> GetProjectNote(string projectUid)
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
}

public class MultipleJobResponse
{
    public IEnumerable<JobDto> Jobs { get; set; }
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
