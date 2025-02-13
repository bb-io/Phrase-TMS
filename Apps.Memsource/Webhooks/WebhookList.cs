using System.Data;
using System.Net;
using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.Models;
using Apps.PhraseTMS.Webhooks.Handlers.OtherHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectTemplateHandlers;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks;

[WebhookList]
public class WebhookList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    #region ProjectWebhooks

    [Webhook("On project created", typeof(ProjectCreationHandler), Description = "On a new project created")]
    public async Task<WebhookResponse<ProjectDto>> ProjectCreation(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectCreatedRequest projectCreatedRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString()!);

        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        if (!string.IsNullOrEmpty(projectCreatedRequest.ProjectNameContains) &&
            !data.Project.Name!.Contains(projectCreatedRequest.ProjectNameContains))
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
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
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
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
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
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
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
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
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
        [WebhookParameter] ProjectOptionalRequest project)
    {
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
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

        if (project.ProjectUId != null && data.Project.UId != project.ProjectUId)
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

    [Webhook("On job created", typeof(JobCreationHandler), Description = "On a new job created")]
    public async Task<WebhookResponse<JobResponse>> JobCreation(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = new()
            {
                Uid = data.JobParts.FirstOrDefault()?.Uid,
                Filename = data.JobParts.FirstOrDefault()?.Filename,
                TargetLanguage = data.JobParts.FirstOrDefault()?.TargetLang,
                Status = data.JobParts.FirstOrDefault()?.Status,
                ProjectUid = data.JobParts.FirstOrDefault()?.Project.UId,
                ProjectName = data.JobParts.FirstOrDefault()?.Project.Name
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job deleted", typeof(JobDeletionHandler), Description = "On any job deleted")]
    public async Task<WebhookResponse<JobResponse>> JobDeletion(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = new()
            {
                Uid = data.JobParts.FirstOrDefault()?.Uid,
                Filename = data.JobParts.FirstOrDefault()?.Filename,
                TargetLanguage = data.JobParts.FirstOrDefault()?.TargetLang,
                Status = data.JobParts.FirstOrDefault()?.Status,
                ProjectUid = data.JobParts.FirstOrDefault()?.Project.UId,
                ProjectName = data.JobParts.FirstOrDefault()?.Project.Name
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On continuous job updated", typeof(JobContinuousUpdatedHandler),
        Description = "On any continuous job updated")]
    public async Task<WebhookResponse<JobResponse>> JobContinuousUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        var result = new JobResponse
        {
            Uid = data.JobParts.FirstOrDefault()?.Uid,
            Filename = data.JobParts.FirstOrDefault()?.Filename,
            TargetLanguage = data.JobParts.FirstOrDefault()?.TargetLang,
            Status = data.JobParts.FirstOrDefault()?.Status,
            ProjectUid = data.JobParts.FirstOrDefault()?.Project.UId,
            ProjectName = data.JobParts.FirstOrDefault()?.Project.Name,
            //DateDue = response.DateDue
        };

        return new()
        {
            HttpResponseMessage = null,
            Result = result,
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.FirstOrDefault()?.Uid != request.JobUId
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On job assigned", typeof(JobAssignedHandler), Description = "On any job assigned")]
    public async Task<WebhookResponse<JobResponse>> JobAssigned(WebhookRequest webhookRequest,
        [WebhookParameter] JobAssignedRequest request,
        [WebhookParameter] OptionalJobRequest job)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        if (request.UserId is not null &&
            data.JobParts.FirstOrDefault().providers.All(x => x.uid != request.UserId))
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        if (job.JobUId != null && data.JobParts.FirstOrDefault()?.Uid != job.JobUId)
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
            Result = new()
            {
                Uid = data.JobParts.FirstOrDefault()?.Uid,
                Filename = data.JobParts.FirstOrDefault()?.Filename,
                TargetLanguage = data.JobParts.FirstOrDefault()?.TargetLang,
                Status = data.JobParts.FirstOrDefault()?.Status,
                ProjectUid = data.JobParts.FirstOrDefault()?.Project.UId,
                ProjectName = data.JobParts.FirstOrDefault()?.Project.Name
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job due date changed", typeof(JobDueDateChangedHandler), Description = "On any job due date changed")]
    public async Task<WebhookResponse<JobResponse>> JobDueDateChanged(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        var result = new JobResponse
        {
            Uid = data.JobParts.FirstOrDefault()?.Uid,
            Filename = data.JobParts.FirstOrDefault()?.Filename,
            TargetLanguage = data.JobParts.FirstOrDefault()?.TargetLang,
            Status = data.JobParts.FirstOrDefault()?.Status,
            ProjectUid = data.JobParts.FirstOrDefault()?.Project.UId,
            ProjectName = data.JobParts.FirstOrDefault()?.Project.Name
            //DateDue = response.DateDue
        };

        return new()
        {
            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
            Result = result,
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.FirstOrDefault()?.Uid != request.JobUId
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On job exported", typeof(JobExportedHandler), Description = "On any job exported")]
    public async Task<WebhookResponse<JobResponse>> JobExported(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        var result = new JobResponse
        {
            Uid = data.JobParts.FirstOrDefault()?.Uid,
            Filename = data.JobParts.FirstOrDefault()?.Filename,
            TargetLanguage = data.JobParts.FirstOrDefault()?.TargetLang,
            Status = data.JobParts.FirstOrDefault()?.Status,
            ProjectUid = data.JobParts.FirstOrDefault()?.Project.UId,
            ProjectName = data.JobParts.FirstOrDefault()?.Project.Name,
            //DateDue = response.DateDue
        };

        return new()
        {
            HttpResponseMessage = null,
            Result = result,
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.FirstOrDefault()?.Uid != request.JobUId
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On job source updated", typeof(JobSourceUpdatedHandler), Description = "On any job source updated")]
    public async Task<WebhookResponse<JobResponse>> JobSourceUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        var result = new JobResponse
        {
            Uid = data.JobParts.FirstOrDefault()?.Uid,
            Filename = data.JobParts.FirstOrDefault()?.Filename,
            TargetLanguage = data.JobParts.FirstOrDefault()?.TargetLang,
            Status = data.JobParts.FirstOrDefault()?.Status,
            ProjectUid = data.JobParts.FirstOrDefault()?.Project.UId,
            ProjectName = data.JobParts.FirstOrDefault().Project.Name,
            //DateDue = response.DateDue
        };

        return new()
        {
            HttpResponseMessage = null,
            Result = result,
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.FirstOrDefault()?.Uid != request.JobUId
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On job status changed", typeof(JobStatusChangedHandler), Description = "On any job status changed")]
    public async Task<WebhookResponse<JobResponse>> JobStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] JobStatusChangedRequest request,
        [WebhookParameter] ProjectOptionalRequest projectOptionalRequest,
        [WebhookParameter] OptionalJobRequest job,
        [WebhookParameter] [Display("Workflow level (number of step)")]
        string? step,
        [WebhookParameter] [Display("Last workflow level?")]
        bool? lastWorkflowLevel,
        [WebhookParameter][Display("Project name contains")] string? projectNameContains)
    {
        if (job != null && projectOptionalRequest == null)
        {
            throw new PluginMisconfigurationException("If Job ID is specified in the inputs so must be project ID");
        }

        if (!String.IsNullOrEmpty(step) && lastWorkflowLevel.HasValue && lastWorkflowLevel.Value)
        {
            throw new PluginMisconfigurationException(
                "Provide either a specifc workflow step or set last workflow level as true, not both");
        }

        var data = JsonConvert.DeserializeObject<JobStatusChangedWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        if (!String.IsNullOrEmpty(projectNameContains) && !data.metadata.project.name.Contains(projectNameContains))
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        bool hasStatus = request?.Status != null && request?.Status.Count() > 0;
        bool hasJob = !string.IsNullOrEmpty(job.JobUId);
        var jobData = new JobData();
        if (!string.IsNullOrEmpty(job.JobUId) && !string.IsNullOrEmpty(projectOptionalRequest.ProjectUId))
        {
            var client = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);
            var apirequest = new PhraseTmsRequest(
                $"/api2/v1/projects/{projectOptionalRequest.ProjectUId}/jobs/{job.JobUId}",
                Method.Get, InvocationContext.AuthenticationCredentialsProviders);
            jobData = await client.ExecuteWithHandling<JobData>(apirequest);


            if (lastWorkflowLevel.HasValue && lastWorkflowLevel.Value)
            {
                step = jobData.LastWorkflowLevel.ToString();
            }
        }

        bool hasWorkflowStep = !string.IsNullOrEmpty(step);
        bool lastStep = lastWorkflowLevel.HasValue && lastWorkflowLevel.Value;

        if (!hasStatus && !hasJob && !hasWorkflowStep && !lastStep)
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = data.JobParts.FirstOrDefault().Uid,
                    Status = data.JobParts.FirstOrDefault().Status,
                    ProjectUid = data.metadata.project.uid,
                    ProjectName = data.metadata.project.name,
                    Filename = data.JobParts.FirstOrDefault().fileName,
                    TargetLanguage = data.JobParts.FirstOrDefault().targetLang
                }
            };
        }

        var Result = new WebhookResponse<JobResponse>();

        switch ((hasStatus, hasJob, hasWorkflowStep, lastStep))
        {
            case (true, true, true, true):
            case (true, true, true, false):
                Result = await HandleFullData(data.JobParts, request.Status, jobData, step);
                break;
            case (true, true, false, false):
                Result = HandleStatusJobOnly(data.JobParts, request.Status, jobData);
                break;
            case (true, false, false, true):
                Result = HandleStatusLastStep(data.JobParts, request.Status);
                break;
            case (true, false, true, false):
                Result = HandleStatusStepOnly(data.JobParts, request.Status, step);
                break;
            case (true, false, false, false):
                Result = HandleStatusOnly(data.JobParts, request.Status);
                break;
            case (false, true, true, true):
            case (false, true, true, false):
                Result = await HandleJobStep(data.JobParts, jobData, step);
                break;
            case (false, true, false, false):
                Result = HandleJobOnly(data.JobParts, jobData);
                break;
            case (false, false, false, true):
                Result = HandleOnlyLastStep(data.JobParts);
                break;
            case (false, false, true, false):
                Result = HandleStepOnly(data.JobParts, step);
                break;
            default: throw new InvalidOperationException("Unexpected case encountered");
        }

        return Result;
    }

    #region Helpers

    private WebhookResponse<JobResponse> HandleStepOnly(List<JobPart> jobParts, string? step)
    {
        if (jobParts.Any(x => x.workflowLevel.ToString() == step))
        {
            var selectedJob = jobParts.FirstOrDefault(x => x.workflowLevel.ToString() == step);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    private WebhookResponse<JobResponse> HandleOnlyLastStep(List<JobPart> jobParts)
    {
        if (jobParts.Any(x => x.workflowLevel == x.project.lastWorkflowLevel))
        {
            var selectedJob = jobParts.FirstOrDefault(x => x.workflowLevel == x.project.lastWorkflowLevel);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    private WebhookResponse<JobResponse> HandleJobOnly(List<JobPart> jobParts, JobData jobData)
    {
        if (jobParts.Any(x => x.Uid == jobData.Uid))
        {
            var selectedJob = jobParts.FirstOrDefault(x => x.Uid == jobData.Uid);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    private async Task<WebhookResponse<JobResponse>> HandleJobStep(List<JobPart> jobParts, JobData jobData,
        string? step)
    {
        if (jobParts.Any(x => x.Uid == jobData.Uid && x.workflowLevel.ToString() == step))
        {
            var selectedJob = jobParts.FirstOrDefault(x => x.Uid == jobData.Uid && x.workflowLevel.ToString() == step);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            string jobIDforStep = await GetJobIDforSpecificStep(jobData.ServerTaskId, step);
            if (jobParts.Any(x => x.Uid == jobIDforStep))
            {
                var selectedJob = jobParts.FirstOrDefault(x => x.Uid == jobIDforStep);
                return new WebhookResponse<JobResponse>
                {
                    HttpResponseMessage = null,
                    Result = new()
                    {
                        Uid = selectedJob.Uid,
                        Status = selectedJob.Status,
                        ProjectUid = selectedJob.project.Uid,
                        Filename = selectedJob.fileName,
                        TargetLanguage = selectedJob.targetLang
                    }
                };
            }

            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    private async Task<string> GetJobIDforSpecificStep(string taskId, string? step)
    {
        var client = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);
        var apirequest = new PhraseTmsRequest($"/api2/v1/mappings/tasks/{taskId}",
            Method.Get, InvocationContext.AuthenticationCredentialsProviders);
        apirequest.AddQueryParameter("workflowLevel", step);
        var response = await client.ExecuteWithHandling<TaskData>(apirequest);
        return response.job.uid;
    }

    private WebhookResponse<JobResponse> HandleStatusOnly(List<JobPart> jobParts, IEnumerable<string>? statuses)
    {
        if (jobParts.Any(x => statuses.Contains(x.Status)))
        {
            var selectedJob = jobParts.FirstOrDefault(x => statuses.Contains(x.Status));
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    private WebhookResponse<JobResponse> HandleStatusStepOnly(List<JobPart> jobParts, IEnumerable<string>? statuses,
        string? step)
    {
        if (jobParts.Any(x => statuses.Contains(x.Status) && x.workflowLevel.ToString() == step))
        {
            var selectedJob =
                jobParts.FirstOrDefault(x => statuses.Contains(x.Status) && x.workflowLevel.ToString() == step);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    private WebhookResponse<JobResponse> HandleStatusLastStep(List<JobPart> jobParts, IEnumerable<string>? statuses)
    {
        if (jobParts.Any(x => statuses.Contains(x.Status) && x.workflowLevel == x.project.lastWorkflowLevel))
        {
            var selectedJob = jobParts.FirstOrDefault(x =>
                statuses.Contains(x.Status) && x.workflowLevel == x.project.lastWorkflowLevel);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    private WebhookResponse<JobResponse> HandleStatusJobOnly(List<JobPart> jobParts, IEnumerable<string>? statuses,
        JobData jobData)
    {
        if (jobParts.Any(x => statuses.Contains(x.Status) && x.Uid == jobData.Uid))
        {
            var selectedJob = jobParts.FirstOrDefault(x => statuses.Contains(x.Status) && x.Uid == jobData.Uid);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    private async Task<WebhookResponse<JobResponse>> HandleFullData(List<JobPart> jobParts,
        IEnumerable<string>? statuses, JobData jobData, string? step)
    {
        if (jobParts.Any(x =>
                statuses.Contains(x.Status) && x.Uid == jobData.Uid && x.workflowLevel.ToString() == step))
        {
            var selectedJob = jobParts.FirstOrDefault(x =>
                statuses.Contains(x.Status) && x.Uid == jobData.Uid && x.workflowLevel.ToString() == step);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.project.Uid,
                    Filename = selectedJob.fileName,
                    TargetLanguage = selectedJob.targetLang
                }
            };
        }
        else
        {
            string jobIDforStep = await GetJobIDforSpecificStep(jobData.ServerTaskId, step);
            if (jobParts.Any(x =>
                    statuses.Contains(x.Status) && x.Uid == jobIDforStep && x.workflowLevel.ToString() == step))
            {
                var selectedJob = jobParts.FirstOrDefault(x =>
                    statuses.Contains(x.Status) && x.Uid == jobIDforStep && x.workflowLevel.ToString() == step);
                return new WebhookResponse<JobResponse>
                {
                    HttpResponseMessage = null,
                    Result = new()
                    {
                        Uid = selectedJob.Uid,
                        Status = selectedJob.Status,
                        ProjectUid = selectedJob.project.Uid,
                        Filename = selectedJob.fileName,
                        TargetLanguage = selectedJob.targetLang
                    }
                };
            }

            return new()
            {
                HttpResponseMessage = null,
                Result = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }
    }

    #endregion

    [Webhook("On all jobs in workflow step reached status", typeof(AllJobsReachedStatusHandler),
        Description =
            "Triggered when all jobs in a specific workflow step reach a specified status. Returns only jobs in the specified workflow step")]
    public async Task<WebhookResponse<JobsResponse>> HandleAllJobsReachedStatusAsync(WebhookRequest webhookRequest,
        [WebhookParameter] WorkflowStepStatusRequest workflowStepStatusRequest)
    {
        var requestBody = webhookRequest.Body.ToString();
        if (string.IsNullOrWhiteSpace(requestBody))
        {
            throw new InvalidCastException("Webhook request body is null or empty.");
        }

        var jobData = JsonConvert.DeserializeObject<JobsWrapper>(requestBody);
        if (jobData is null)
        {
            throw new InvalidCastException("Failed to deserialize webhook request body.");
        }

        var primaryJob = jobData.JobParts.FirstOrDefault();
        if (primaryJob?.Status != workflowStepStatusRequest.JobStatus)
        {
            return new WebhookResponse<JobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        var phraseTmsClient = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);
        var jobsEndpoint = $"/api2/v2/projects/{workflowStepStatusRequest.ProjectUId}/jobs";
        var apiRequest =
            new PhraseTmsRequest(jobsEndpoint, Method.Get, InvocationContext.AuthenticationCredentialsProviders);

        var allJobs = await phraseTmsClient.Paginate<JobDto>(apiRequest);
        if (allJobs.All(job => primaryJob?.Uid != job.Uid))
        {
            return new WebhookResponse<JobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        allJobs.ForEach(job => job.Project = new()
        {
            UId = workflowStepStatusRequest.ProjectUId
        });

        var relevantJobs = allJobs
            .Where(job => job.WorkflowStep?.Id == workflowStepStatusRequest.WorkflowStepId)
            .ToList();

        var allJobsInStepMatchStatus = relevantJobs.All(job => job.Status == workflowStepStatusRequest.JobStatus);
        var triggered = allJobsInStepMatchStatus;

        return new WebhookResponse<JobsResponse>
        {
            ReceivedWebhookRequestType = triggered ? WebhookRequestType.Default : WebhookRequestType.Preflight,
            Result = new JobsResponse
            {
                Jobs = relevantJobs.Select(job => new JobResponse
                {
                    Uid = job.Uid,
                    ProjectUid = job.Project.UId,
                    ProjectName = job.Project.Name,
                    Filename = job.Filename,
                    SourceLanguage = job.SourceLang,
                    Status = job.Status,
                    TargetLanguage = job.TargetLang,
                    WordCount = job.WordsCount
                }).ToList()
            }
        };
    }

    [Webhook("On job target updated", typeof(JobTargetUpdatedHandler), Description = "On any job target updated")]
    public async Task<WebhookResponse<JobResponse>> JobTargetUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        var result = new JobResponse
        {
            Uid = data.JobPart.Uid,
            Filename = data.JobPart.Filename,
            TargetLanguage = data.JobPart.TargetLang,
            Status = data.JobPart.Status,
            ProjectUid = data.JobPart.Project.UId,
            ProjectName = data.JobPart.Project?.Name ?? string.Empty,
            //DateDue = response.DateDue
        };

        return new()
        {
            HttpResponseMessage = null,
            Result = result,
            ReceivedWebhookRequestType = request.JobUId != null && data.JobPart.Uid != request.JobUId
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    [Webhook("On job unexported", typeof(JobUnexportedHandler), Description = "On any job unexported")]
    public async Task<WebhookResponse<JobResponse>> JobUnexported(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        var result = new JobResponse
        {
            Uid = data.JobParts.FirstOrDefault()?.Uid,
            Filename = data.JobParts.FirstOrDefault()?.Filename,
            TargetLanguage = data.JobParts.FirstOrDefault()?.TargetLang,
            Status = data.JobParts.FirstOrDefault()?.Status,
            ProjectUid = data.JobParts.FirstOrDefault()?.Project.UId,
            ProjectName = data.JobParts.FirstOrDefault()?.Project.Name,
            //DateDue = response.DateDue
        };

        return new()
        {
            HttpResponseMessage = null,
            Result = result,
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.FirstOrDefault()?.Uid != request.JobUId
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    #endregion

    #region ProjectTemplates

    [Webhook("On template created", typeof(TemplateCreationHandler), Description = "On a new template created")]
    public async Task<WebhookResponse<ProjectTemplateDto>> ProjectTemplateCreation(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.ProjectTemplate
        };
    }

    [Webhook("On template deleted", typeof(TemplateDeletionHandler), Description = "On any template deleted")]
    public async Task<WebhookResponse<ProjectTemplateDto>> ProjectTemplateDeletion(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.ProjectTemplate
        };
    }

    [Webhook("On template updated", typeof(TemplateUpdatedHandler), Description = "On any template updated")]
    public async Task<WebhookResponse<ProjectTemplateDto>> ProjectTemplateUpdated(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        return new()
        {
            HttpResponseMessage = null,
            Result = data.ProjectTemplate
        };
    }

    #endregion

    #region OtherWebhooks

    [Webhook("On analysis created", typeof(AnalysisCreationHandler), Description = "On a new analysis created")]
    public async Task<WebhookResponse<AnalysisDto>> AnalysisCreation(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<AnalyseWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
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
    public List<JobDto> JobParts { get; set; }
}

public class JobWrapper
{
    public JobDto JobPart { get; set; }
}

public class ProjectTemplateWrapper
{
    public ProjectTemplateDto ProjectTemplate { get; set; }
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

public class JobPart
{
    public string Uid { get; set; }
    public int workflowLevel { get; set; }
    public string Status { get; set; }
    [JsonProperty("project")] public _Project? project { get; set; }

    public string? task { get; set; }
    public string? fileName { get; set; }
    public string? targetLang { get; set; }
}

public class _Project
{
    [JsonProperty("uid")] public string Uid { get; set; }

    public int? lastWorkflowLevel { get; set; }
}

public class JobStatusChangedWrapper
{
    public List<JobPart> JobParts { get; set; }
    [JsonProperty("metadata")] public projectMetadata metadata { get; set; }
}

public class projectMetadata
{
    public Project project { get; set; }
}

public class Project
{
    public string uid { get; set; }
    public int lastWorkflowLevel { get; set; }
    public string name { get; set; }
}