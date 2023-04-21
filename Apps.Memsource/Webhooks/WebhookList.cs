using Apps.PhraseTms.Dtos;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.OtherHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;
using Apps.PhraseTMS.Webhooks.Handlers.ProjectTemplateHandlers;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Text.Json;

namespace Apps.PhraseTMS.Webhooks
{
    [WebhookList]
    public class WebhookList 
    {
        #region ProjectWebhooks

        [Webhook(typeof(ProjectCreationHandler))]
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

        [Webhook(typeof(ProjectDeletionHandler))]
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

        [Webhook(typeof(ProjectDueDateChangedHandler))]
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

        [Webhook(typeof(ProjectMetadataUpdatedHandler))]
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

        [Webhook(typeof(ProjectSharedAssignedHandler))]
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

        [Webhook(typeof(ProjectStatusChangedHandler))]
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

        [Webhook(typeof(JobCreationHandler))]
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

        [Webhook(typeof(JobDeletionHandler))]
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

        [Webhook(typeof(JobContinuousUpdatedHandler))]
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

        [Webhook(typeof(JobAssignedHandler))]
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

        [Webhook(typeof(JobDueDateChangedHandler))]
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

        [Webhook(typeof(JobExportedHandler))]
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

        [Webhook(typeof(JobSourceUpdatedHandler))]
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

        [Webhook(typeof(JobStatusChangedHandler))]
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

        [Webhook(typeof(JobTargetUpdatedHandler))]
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

        [Webhook(typeof(JobUnexportedHandler))]
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

        [Webhook(typeof(TemplateCreationHandler))]
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

        [Webhook(typeof(TemplateDeletionHandler))]
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

        [Webhook(typeof(TemplateUpdatedHandler))]
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

        [Webhook(typeof(AnalysisCreationHandler))]
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
