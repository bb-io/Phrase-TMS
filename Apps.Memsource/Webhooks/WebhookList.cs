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
        public /*async Task<WebhookResponse<*/ProjectDto ProjectDeletion(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.Project; //new WebhookResponse<ProjectDto>
            //{
            //    HttpResponseMessage = null,
            //    Result = data.Project
            //};
        }

        [Webhook(typeof(ProjectDueDateChangedHandler))]
        public /*async Task<WebhookResponse<*/ProjectDto ProjectDueDateChanged(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.Project; //new WebhookResponse<ProjectDto>
            //{
            //    HttpResponseMessage = null,
            //    Result = data.Project
            //};
        }

        [Webhook(typeof(ProjectMetadataUpdatedHandler))]
        public /*async Task<WebhookResponse<*/ProjectDto ProjectMetadataUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.Project; //new WebhookResponse<ProjectDto>
            //{
            //    HttpResponseMessage = null,
            //    Result = data.Project
            //};
        }

        [Webhook(typeof(ProjectSharedAssignedHandler))]
        public /*async Task<WebhookResponse<*/ProjectDto ProjectSharedAssigned(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.Project; //new WebhookResponse<ProjectDto>
            //{
            //    HttpResponseMessage = null,
            //    Result = data.Project
            //};
        }

        [Webhook(typeof(ProjectStatusChangedHandler))]
        public /*async Task<WebhookResponse<*/ProjectDto ProjectStatusChanged(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.Project; //new WebhookResponse<ProjectDto>
            //{
            //    HttpResponseMessage = null,
            //    Result = data.Project
            //};
        }

        #endregion

        #region JobWebhooks

        [Webhook(typeof(JobCreationHandler))]
        public JobDto JobCreation(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobDeletionHandler))]
        public JobDto JobDeletion(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobContinuousUpdatedHandler))]
        public JobDto JobContinuousUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobAssignedHandler))]
        public JobDto JobAssigned(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobDueDateChangedHandler))]
        public JobDto JobDueDateChanged(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobExportedHandler))]
        public JobDto JobExported(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobSourceUpdatedHandler))]
        public JobDto JobSourceUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobStatusChangedHandler))]
        public JobDto JobStatusChanged(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobTargetUpdatedHandler))]
        public JobDto JobTargetUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        [Webhook(typeof(JobUnexportedHandler))]
        public JobDto JobUnexported(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<JobsWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.JobParts.First();
        }

        #endregion

        #region ProjectTemplates

        [Webhook(typeof(TemplateCreationHandler))]
        public ProjectTemplateDto ProjectTemplateCreation(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.ProjectTemplate;
           
        }

        [Webhook(typeof(TemplateDeletionHandler))]
        public ProjectTemplateDto ProjectTemplateDeletion(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.ProjectTemplate;
        }

        [Webhook(typeof(TemplateUpdatedHandler))]
        public ProjectTemplateDto ProjectTemplateUpdated(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<ProjectTemplateWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.ProjectTemplate;
        }

        #endregion

        #region OtherWebhooks

        [Webhook(typeof(AnalysisCreationHandler))]
        public AnalysisDto AnalysisCreation(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<AnalyseWrapper>(webhookRequest.Body.ToString());
            if (data is null)
            {
                throw new InvalidCastException(nameof(webhookRequest.Body));
            }
            return data.Analyse;

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
