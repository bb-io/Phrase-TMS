using System.Data;
using System.Linq;
using System.Net;
using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Dtos.Analysis;
using Apps.PhraseTMS.Dtos.Jobs;
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
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using RestSharp;
using static Apps.PhraseTMS.Webhooks.WebhookList;

namespace Apps.PhraseTMS.Webhooks;

[WebhookList]
public class WebhookList(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
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
        [WebhookParameter] ProjectOptionalRequest project,
        [WebhookParameter][Display("Project name contains")] string? projectNameContains)
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

        if (!String.IsNullOrEmpty(projectNameContains) && !data.Project.Name.Contains(projectNameContains))
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
    public async Task<WebhookResponse<MultipleJobResponse>> JobCreation(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        return new()
        {
            Result = await FetchJobs(data),
        };
    }

    [Webhook("On jobs deleted", typeof(JobDeletionHandler), Description = "Triggered when any jobs are deleted")]
    public async Task<WebhookResponse<JobsWrapper>> JobDeletion(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        return new()
        {
            Result = data
        };
    }

    [Webhook("On continuous jobs updated", typeof(JobContinuousUpdatedHandler), Description = "Triggered when continuous jobs are updated")]
    public async Task<WebhookResponse<MultipleJobResponse>> JobContinuousUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
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
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());

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
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
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
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
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
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
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
        //[WebhookParameter] ProjectOptionalRequest projectOptionalRequest,
        [WebhookParameter] OptionalJobRequest job,
        [WebhookParameter] WorkflowStepOptionalRequest workflowStepRequest,
        [WebhookParameter] OptionalSourceFileIdRequest sourceFileId,
        [WebhookParameter] OptionalSearchJobsQuery jobsQuery,
        //[WebhookParameter] [Display("Workflow level (number of step)")] string? step,
        //[WebhookParameter] [Display("Last workflow level?")] bool? lastWorkflowLevel,
        [WebhookParameter][Display("Project name contains")] string? projectNameContains)
    {
        //if (job?.JobUId != null && projectOptionalRequest?.ProjectUId == null)
        //{
        //    throw new PluginMisconfigurationException("If Job ID is specified in the inputs you must also specify the Project ID");
        //}

        //if (sourceFileId?.SourceFileId != null && projectOptionalRequest?.ProjectUId == null)
        //{
        //    throw new PluginMisconfigurationException("If Source file ID is specified in the inputs you must also specify the Project ID");
        //}

        //if (!String.IsNullOrEmpty(step) && lastWorkflowLevel.HasValue && lastWorkflowLevel.Value)
        //{
        //    throw new PluginMisconfigurationException(
        //        "Provide either a specifc workflow step or set last workflow level as true, not both");
        //}

        var data = JsonConvert.DeserializeObject<JobStatusChangedWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
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

        var projectId = data.metadata.project.Uid;

        //bool hasStatus = request?.Status != null && request?.Status.Count() > 0;
        //bool hasJob = !string.IsNullOrEmpty(job.JobUId);
        //var jobData = new JobData();
        IEnumerable<JobPart> selectedJobs = data.JobParts;
        if (!string.IsNullOrEmpty(job.JobUId) && !string.IsNullOrEmpty(projectId))
        {
            selectedJobs = data.JobParts.Where(x => x.Uid == job.JobUId);

            //var apirequest = new RestRequest($"/api2/v1/projects/{projectOptionalRequest.ProjectUId}/jobs/{job.JobUId}", Method.Get);
            //jobData = await Client.ExecuteWithHandling<JobData>(apirequest);

            //if (lastWorkflowLevel.HasValue && lastWorkflowLevel.Value)
            //{
            //    step = jobData.LastWorkflowLevel.ToString();
            //}
        }

        if (!string.IsNullOrEmpty(sourceFileId.SourceFileId) && !string.IsNullOrEmpty(projectId))
        {
            if (workflowStepRequest.WorkflowStepId != null)
            {
                jobsQuery.WorkflowLevel = await Client.GetWorkflowstepLevel(projectId, workflowStepRequest.WorkflowStepId);
            }
            var endpoint = $"/api2/v2/projects/{projectId}/jobs";
            var listJobsRequest = new RestRequest(endpoint.WithQuery(jobsQuery), Method.Get);
            var listJobsResponse = await Client.Paginate<ListJobDto>(listJobsRequest);
            var filteredJobIds = listJobsResponse.Where(x => x.SourceFileUid == sourceFileId.SourceFileId).Select(x => x.Uid);
            selectedJobs = data.JobParts.IntersectBy(filteredJobIds, x => x.Uid);
        }

        if (request?.Status != null && request?.Status.Count() > 0)
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

        return new WebhookResponse<JobResponse>
        {
            HttpResponseMessage = null,
            Result = new()
            {
                Uid = selectedJob.Uid,
                Status = selectedJob.Status,
                ProjectUid = selectedJob.Project.Uid,
                Filename = selectedJob.FileName,
                TargetLanguage = selectedJob.TargetLang,
                ProjectName = data.metadata.project.Name
            }
        };

        //bool hasWorkflowStep = !string.IsNullOrEmpty(step);
        //bool lastStep = lastWorkflowLevel.HasValue && lastWorkflowLevel.Value;

        //if (!hasStatus && !hasJob && !hasWorkflowStep && !lastStep)
        //{
        //    return new()
        //    {
        //        HttpResponseMessage = null,
        //        Result = new()
        //        {
        //            Uid = data.JobParts.FirstOrDefault().Uid,
        //            Status = data.JobParts.FirstOrDefault().Status,
        //            ProjectUid = data.metadata.project.Uid,
        //            ProjectName = data.metadata.project.Name,
        //            Filename = data.JobParts.FirstOrDefault().FileName,
        //            TargetLanguage = data.JobParts.FirstOrDefault().TargetLang
        //        }
        //    };
        //}

        var result = new WebhookResponse<JobResponse>();

        //switch ((hasStatus, hasJob, hasWorkflowStep, lastStep))
        //{
        //    case (true, true, true, true):
        //    case (true, true, true, false):
        //        result = await HandleFullData(data.JobParts, request.Status, jobData, step);
        //        break;
        //    case (true, true, false, false):
        //        result = HandleStatusJobOnly(data.JobParts, request.Status, jobData);
        //        break;
        //    case (true, false, false, true):
        //        result = HandleStatusLastStep(data.JobParts, request.Status);
        //        break;
        //    case (true, false, true, false):
        //        result = HandleStatusStepOnly(data.JobParts, request.Status, step);
        //        break;
        //    case (true, false, false, false):
        //        result = HandleStatusOnly(data.JobParts, request.Status);
        //        break;
        //    case (false, true, true, true):
        //    case (false, true, true, false):
        //        result = await HandleJobStep(data.JobParts, jobData, step);
        //        break;
        //    case (false, true, false, false):
        //        result = HandleJobOnly(data.JobParts, jobData);
        //        break;
        //    case (false, false, false, true):
        //        result = HandleOnlyLastStep(data.JobParts);
        //        break;
        //    case (false, false, true, false):
        //        result = HandleStepOnly(data.JobParts, step);
        //        break;
        //    default: throw new InvalidOperationException("Unexpected case encountered");
        //}

        if (result.Result != null)
        {
            result.Result.ProjectName = data.metadata.project.Name;
        }

        return result;
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
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
        if (jobParts.Any(x => x.workflowLevel == x.Project.LastWorkflowLevel))
        {
            var selectedJob = jobParts.FirstOrDefault(x => x.workflowLevel == x.Project.LastWorkflowLevel);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
                        ProjectUid = selectedJob.Project.Uid,
                        Filename = selectedJob.FileName,
                        TargetLanguage = selectedJob.TargetLang
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
        var apirequest = new RestRequest($"/api2/v1/mappings/tasks/{taskId}", Method.Get);
        apirequest.AddQueryParameter("workflowLevel", step);
        var response = await Client.ExecuteWithHandling<TaskData>(apirequest);
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
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
        if (jobParts.Any(x => statuses.Contains(x.Status) && x.workflowLevel == x.Project.LastWorkflowLevel))
        {
            var selectedJob = jobParts.FirstOrDefault(x =>
                statuses.Contains(x.Status) && x.workflowLevel == x.Project.LastWorkflowLevel);
            return new WebhookResponse<JobResponse>
            {
                HttpResponseMessage = null,
                Result = new()
                {
                    Uid = selectedJob.Uid,
                    Status = selectedJob.Status,
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
                    ProjectUid = selectedJob.Project.Uid,
                    Filename = selectedJob.FileName,
                    TargetLanguage = selectedJob.TargetLang
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
                        ProjectUid = selectedJob.Project.Uid,
                        Filename = selectedJob.FileName,
                        TargetLanguage = selectedJob.TargetLang
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
    public async Task<WebhookResponse<ListAllJobsResponse>> HandleAllJobsReachedStatusAsync(WebhookRequest webhookRequest,
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
            return new WebhookResponse<ListAllJobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        var jobsEndpoint = $"/api2/v2/projects/{workflowStepStatusRequest.ProjectUId}/jobs";
        var apiRequest = new RestRequest(jobsEndpoint, Method.Get);

        var allJobs = await Client.Paginate<ListJobDto>(apiRequest);
        if (allJobs.All(job => primaryJob?.Uid != job.Uid))
        {
            return new WebhookResponse<ListAllJobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

        var relevantJobs = allJobs
            .Where(job => job.WorkflowStep?.Id == workflowStepStatusRequest.WorkflowStepId)
            .ToList();

        var allJobsInStepMatchStatus = relevantJobs.All(job => job.Status == workflowStepStatusRequest.JobStatus);
        var triggered = allJobsInStepMatchStatus;

        return new WebhookResponse<ListAllJobsResponse>
        {
            ReceivedWebhookRequestType = triggered ? WebhookRequestType.Default : WebhookRequestType.Preflight,
            Result = new ListAllJobsResponse { Jobs = relevantJobs },
        };
    }

    [Webhook("On job target updated", typeof(JobTargetUpdatedHandler), Description = "Triggered when a job's target has been updated")]
    public async Task<WebhookResponse<JobDto>> JobTargetUpdated(WebhookRequest webhookRequest,
        [WebhookParameter] JobOptionalRequest optionalRequest)
    {
        var data = JsonConvert.DeserializeObject<JobWrapper>(webhookRequest.Body.ToString());
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
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        return new()
        {
            HttpResponseMessage = null,
            Result = await FetchJobs(data),
            ReceivedWebhookRequestType = request.JobUId != null && data.JobParts.All(x => x.Uid != request.JobUId)
                ? WebhookRequestType.Preflight
                : WebhookRequestType.Default
        };
    }

    #endregion

    #region OtherWebhooks

    [Webhook("On analysis created", typeof(AnalysisCreationHandler), Description = "Trigered when a new analysis has been created")]
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
}