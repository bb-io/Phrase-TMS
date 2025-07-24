using System.Data;
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
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks;

[WebhookList]
public class WebhookList(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    #region ProjectWebhooks

    [Webhook("On project created", typeof(ProjectCreationHandler), Description = "On a new project created")]
    public async Task<WebhookResponse<ProjectDto>> ProjectCreation(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectCreatedRequest projectCreatedRequest,
        [WebhookParameter] MultipleSubdomains subdomains,
        [WebhookParameter] MultipleDomains domains)
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
        [WebhookParameter] [Display("Project name contains")] string? projectNameContains,
        [WebhookParameter] [Display("Project name doesn't contains")] string? projectNameDoesntContains,
        [WebhookParameter] MultipleSubdomains subdomains,
        [WebhookParameter] MultipleDomains domains)
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
        [WebhookParameter] ProjectOptionalRequest projectOptionalRequest,
        [WebhookParameter] OptionalJobRequest job,
        [WebhookParameter] WorkflowStepOptionalRequest workflowStepRequest,
        [WebhookParameter] OptionalSourceFileIdRequest sourceFileId,
        [WebhookParameter] OptionalSearchJobsQuery jobsQuery,
        [WebhookParameter] [Display("Last workflow level?")] bool? lastWorkflowLevel,
        [WebhookParameter] [Display("Project name contains")] string? projectNameContains,
        [WebhookParameter] [Display("Project name doesn't contains")] string? projectNameDoesntContains,
        [WebhookParameter] MultipleSubdomains subdomains)
    {
        InvocationContext.Logger?.LogInformation("[Phrase TMS WebhookLogger] Invoked webhook", null);

        InvocationContext.Logger?.LogInformation( $"[Phrase TMS WebhookLogger] Input parameters: request.Status={JsonConvert.SerializeObject(request?.Status)}", null);

        if (job?.JobUId != null && projectOptionalRequest?.ProjectUId == null)
        {          
            throw new PluginMisconfigurationException("If Job ID is specified in the inputs you must also specify the Project ID");
        }

        if (sourceFileId?.SourceFileId != null && projectOptionalRequest?.ProjectUId == null)
        {
            throw new PluginMisconfigurationException("If Source file ID is specified in the inputs you must also specify the Project ID");
        }

        var data = JsonConvert.DeserializeObject<JobStatusChangedWrapper>(webhookRequest.Body.ToString());
        var payloadJson = JsonConvert.SerializeObject(data, Formatting.Indented);
        InvocationContext.Logger?.LogInformation(
            $"[Phrase TMS WebhookLogger] Payload received from server JSON: {payloadJson}", null);

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

        IEnumerable<JobPart> selectedJobs = data.JobParts;

        var payloadJson2 = JsonConvert.SerializeObject(selectedJobs, Formatting.Indented);
        InvocationContext.Logger?.LogInformation(
            $"[Phrase TMS WebhookLogger] Payload JSON Job parts: {payloadJson2}", null);

        var projectId = data.metadata.project.Uid;
        int workflowLevel = 0;

        if (workflowStepRequest != null && !String.IsNullOrEmpty(workflowStepRequest?.WorkflowStepId))
        {
            workflowLevel = await Client.GetWorkflowstepLevel(projectId, workflowStepRequest.WorkflowStepId);
        }
        else if (lastWorkflowLevel.HasValue && lastWorkflowLevel.Value)
        {
            workflowLevel = await Client.GetLastWorkflowstepLevel(projectId);
        }

        if (string.IsNullOrEmpty(job?.JobUId) && workflowLevel > 0)
        {
            if (workflowStepRequest != null && !String.IsNullOrEmpty(workflowStepRequest?.WorkflowStepId))
            {
                if (selectedJobs.Any(x => x.workflowLevel == workflowLevel))
                {
                    selectedJobs = selectedJobs.Where(x => x.workflowLevel == workflowLevel);
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
            else if (lastWorkflowLevel.HasValue && lastWorkflowLevel.Value)
            {
                if (selectedJobs.Any(x => x.workflowLevel == workflowLevel))
                {
                    selectedJobs = selectedJobs.Where(x => x.workflowLevel == workflowLevel);
                }
            }
        }

        if (!string.IsNullOrEmpty(job?.JobUId) && !string.IsNullOrEmpty(projectId))
        {
            selectedJobs = data.JobParts.Where(x => x.Uid == job.JobUId);
        }
        else if (!string.IsNullOrEmpty(sourceFileId?.SourceFileId) && !string.IsNullOrEmpty(projectId))
        {
            var endpoint = $"/api2/v2/projects/{projectId}/jobs";
            var listJobsRequest = new RestRequest(endpoint.WithQuery(jobsQuery), Method.Get);
            if (workflowLevel > 0)
            {
                listJobsRequest.AddQueryParameter("workflowLevel", workflowLevel);
            }
            var listJobsResponse = await Client.Paginate<ListJobDto>(listJobsRequest);
            var filteredJobIds = listJobsResponse.Where(x => x.SourceFileUid == sourceFileId.SourceFileId).Select(x => x.Uid);
            selectedJobs = data.JobParts.IntersectBy(filteredJobIds, x => x.Uid);
        }

        if (jobsQuery.TargetLang != null)
        {
            selectedJobs = selectedJobs.Where(x => x.TargetLang == jobsQuery.TargetLang);
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

        var response= new WebhookResponse<JobResponse>
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

        var payloadJson3 = JsonConvert.SerializeObject(response, Formatting.Indented);
        InvocationContext.Logger?.LogInformation(
            $"[Phrase TMS WebhookLogger] Payload JSON response: {payloadJson3}", null);

        return response;
    }

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
        if (primaryJob == null)
        {
            throw new InvalidCastException("Failed to retrieve a job from webhook request body.");
        }

        if (primaryJob.Status != workflowStepStatusRequest.JobStatus)
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

        var jobsEndpoint = $"/api2/v2/projects/{workflowStepStatusRequest.ProjectUId}/jobs";
        var apiRequest = new RestRequest(jobsEndpoint, Method.Get);
        apiRequest.AddQueryParameter("workflowLevel", primaryJob.workflowLevel);

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
            .Where(job => job.Status == workflowStepStatusRequest.JobStatus)
            .ToList();

        if (allJobs.Count != relevantJobs.Count)
        {
            return new WebhookResponse<ListAllJobsResponse>
            {
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };
        }

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

    public subdomain subDomain { get; set; }
}

public class subdomain
{
    [Display("Subdomain name")]
    [JsonProperty("name")] public string subDomainName { get; set; }

    [Display("Subdomain Uid")]
    [JsonProperty("Uid")] public string subDomainUid { get; set; }
}