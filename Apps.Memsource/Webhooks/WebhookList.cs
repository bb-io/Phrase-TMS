using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.OtherHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectTemplateHandlers;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Webhooks;

[WebhookList]
public class WebhookList 
{
    #region ProjectWebhooks

    [Webhook("On project created", typeof(ProjectCreationHandler), Description = "On a new project created")]
    public async Task<WebhookResponse<ProjectDto>> ProjectCreation(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if(data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On project deleted", typeof(ProjectDeletionHandler), Description = "On any project deleted")]
    public async Task<WebhookResponse<ProjectDto>> ProjectDeletion(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On project due date changed", typeof(ProjectDueDateChangedHandler), Description = "On any project due date changed")]
    public async Task<WebhookResponse<ProjectDto>> ProjectDueDateChanged(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On project metadata updated", typeof(ProjectMetadataUpdatedHandler), Description = "On any project metadata updated")]
    public async Task<WebhookResponse<ProjectDto>> ProjectMetadataUpdated(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On shared project assigned", typeof(ProjectSharedAssignedHandler), Description = "On any shared project assigned")]
    public async Task<WebhookResponse<ProjectDto>> ProjectSharedAssigned(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        return new()
        {
            HttpResponseMessage = null,
            Result = data.Project
        };
    }

    [Webhook("On project status changed", typeof(ProjectStatusChangedHandler), Description = "On any project status changed")]
    public async Task<WebhookResponse<ProjectDto>> ProjectStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] ProjectStatusChangedRequest request)
    {
        var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        
        if(request.Status is not null && data.Project.Status != request.Status)
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
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On continuous job updated", typeof(JobContinuousUpdatedHandler), Description = "On any continuous job updated")]
    public async Task<WebhookResponse<JobResponse>> JobContinuousUpdated(WebhookRequest webhookRequest)
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
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job assigned", typeof(JobAssignedHandler), Description = "On any job assigned")]
    public async Task<WebhookResponse<JobResponse>> JobAssigned(WebhookRequest webhookRequest, [WebhookParameter] JobAssignedRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        if (request.UserId is not null && data.JobParts.FirstOrDefault().AssignedTo.All(x => x.Linguist.UId != request.UserId))
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
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job due date changed", typeof(JobDueDateChangedHandler), Description = "On any job due date changed")]
    public async Task<WebhookResponse<JobResponse>> JobDueDateChanged(WebhookRequest webhookRequest)
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
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job exported", typeof(JobExportedHandler), Description = "On any job exported")]
    public async Task<WebhookResponse<JobResponse>> JobExported(WebhookRequest webhookRequest)
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
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job source updated", typeof(JobSourceUpdatedHandler), Description = "On any job source updated")]
    public async Task<WebhookResponse<JobResponse>> JobSourceUpdated(WebhookRequest webhookRequest)
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
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job status changed", typeof(JobStatusChangedHandler), Description = "On any job status changed")]
    public async Task<WebhookResponse<JobResponse>> JobStatusChanged(WebhookRequest webhookRequest,
        [WebhookParameter] JobStatusChangedRequest request)
    {
        var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        
        if(request.Status is not null && data.JobParts.FirstOrDefault()?.Status != request.Status)
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
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job target updated", typeof(JobTargetUpdatedHandler), Description = "On any job target updated")]
    public async Task<WebhookResponse<JobResponse>> JobTargetUpdated(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<JobWrapper>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }
        return new()
        {
            HttpResponseMessage = null,
            Result = new()
            {
                Uid = data.JobPart.Uid,
                Filename = data.JobPart.Filename,
                TargetLanguage = data.JobPart.TargetLang,
                Status = data.JobPart.Status,
                ProjectUid = data.JobPart.Project.UId,
                //DateDue = response.DateDue
            }
        };
    }

    [Webhook("On job unexported", typeof(JobUnexportedHandler), Description = "On any job unexported")]
    public async Task<WebhookResponse<JobResponse>> JobUnexported(WebhookRequest webhookRequest)
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
                //DateDue = response.DateDue
            }
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