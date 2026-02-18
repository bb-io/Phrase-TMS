using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Webhooks.Models.Requests
{
    public class ProjectWebhookRequest
    {
        [Display("Project ID")]
        [DataSource(typeof(ProjectDataHandler))]
        public string? ProjectUId { get; set; } = string.Empty;
    }
}
