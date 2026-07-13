using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class PreTranslationFinishedRequest
{
    [Display("Job ID",
        Description =
            "Select a job when Project ID is provided, or enter the job ID manually when Project ID is empty.")]
    [DataSource(typeof(WebhookJobDataHandler))]
    public string JobUId { get; set; } = string.Empty;
}
