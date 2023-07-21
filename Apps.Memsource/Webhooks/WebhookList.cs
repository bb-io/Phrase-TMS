using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.OtherHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectTemplateHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Webhooks
{
    [WebhookList]
    public class WebhookList 
    {
        #region ProjectWebhooks

        [Webhook("On project created", typeof(ProjectCreationHandler), Description = "On project created")]
        public async Task<WebhookResponse<ProjectDto>> ProjectCreation(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if(data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = null,
                Result = data.Project
            };
        }

        [Webhook("On project deleted", typeof(ProjectDeletionHandler), Description = "On project deleted")]
        public async Task<WebhookResponse<ProjectDto>> ProjectDeletion(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = null,
                Result = data.Project
            };
        }

        [Webhook("On project due date changed", typeof(ProjectDueDateChangedHandler), Description = "On project due date changed")]
        public async Task<WebhookResponse<ProjectDto>> ProjectDueDateChanged(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = null,
                Result = data.Project
            };
        }

        [Webhook("On project metadata updated", typeof(ProjectMetadataUpdatedHandler), Description = "On project metadata updated")]
        public async Task<WebhookResponse<ProjectDto>> ProjectMetadataUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = null,
                Result = data.Project
            };
        }

        [Webhook("On shared project assigned", typeof(ProjectSharedAssignedHandler), Description = "On shared project assigned")]
        public async Task<WebhookResponse<ProjectDto>> ProjectSharedAssigned(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = null,
                Result = data.Project
            };
        }

        [Webhook("On project status changed", typeof(ProjectStatusChangedHandler), Description = "On project status changed")]
        public async Task<WebhookResponse<ProjectDto>> ProjectStatusChanged(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectDto>
            {
                HttpResponseMessage = null,
                Result = data.Project
            };
        }

        #endregion

        #region JobWebhooks

        [Webhook("On job created", typeof(JobCreationHandler), Description = "On job created")]
        public async Task<WebhookResponse<JobDto>> JobCreation(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On job deleted", typeof(JobDeletionHandler), Description = "On job deleted")]
        public async Task<WebhookResponse<JobDto>> JobDeletion(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On continuous job updated", typeof(JobContinuousUpdatedHandler), Description = "On continuous job updated")]
        public async Task<WebhookResponse<JobDto>> JobContinuousUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On job assigned", typeof(JobAssignedHandler), Description = "On job assigned")]
        public async Task<WebhookResponse<JobDto>> JobAssigned(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On job due date changed", typeof(JobDueDateChangedHandler), Description = "On job due date changed")]
        public async Task<WebhookResponse<JobDto>> JobDueDateChanged(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On job exported", typeof(JobExportedHandler), Description = "On job exported")]
        public async Task<WebhookResponse<JobDto>> JobExported(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On job source updated", typeof(JobSourceUpdatedHandler), Description = "On job source updated")]
        public async Task<WebhookResponse<JobDto>> JobSourceUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On job status changed", typeof(JobStatusChangedHandler), Description = "On job status changed")]
        public async Task<WebhookResponse<JobDto>> JobStatusChanged(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On job target updated", typeof(JobTargetUpdatedHandler), Description = "On job target updated")]
        public async Task<WebhookResponse<JobDto>> JobTargetUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        [Webhook("On job unexported", typeof(JobUnexportedHandler), Description = "On job unexported")]
        public async Task<WebhookResponse<JobDto>> JobUnexported(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<JobDto>
            {
                HttpResponseMessage = null,
                Result = data.JobParts.First()
            };
        }

        #endregion

        #region ProjectTemplates

        [Webhook("On template created", typeof(TemplateCreationHandler), Description = "On template created")]
        public async Task<WebhookResponse<ProjectTemplateDto>> ProjectTemplateCreation(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectTemplateDto>
            {
                HttpResponseMessage = null,
                Result = data.ProjectTemplate
            };
        }

        [Webhook("On template deleted", typeof(TemplateDeletionHandler), Description = "On template deleted")]
        public async Task<WebhookResponse<ProjectTemplateDto>> ProjectTemplateDeletion(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectTemplateDto>
            {
                HttpResponseMessage = null,
                Result = data.ProjectTemplate
            };
        }

        [Webhook("On template updated", typeof(TemplateUpdatedHandler), Description = "On template updated")]
        public async Task<WebhookResponse<ProjectTemplateDto>> ProjectTemplateUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<ProjectTemplateDto>
            {
                HttpResponseMessage = null,
                Result = data.ProjectTemplate
            };
        }

        #endregion

        #region OtherWebhooks

        [Webhook("On analysis created", typeof(AnalysisCreationHandler), Description = "On analysis created")]
        public async Task<WebhookResponse<AnalysisDto>> AnalysisCreation(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<AnalyseWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return new WebhookResponse<AnalysisDto>
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

    public class ProjectTemplateWrapper
    {
        public ProjectTemplateDto ProjectTemplate { get; set; }
    }

    public class AnalyseWrapper
    {
        public AnalysisDto Analyse { get; set; }
    }
}
