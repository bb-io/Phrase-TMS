using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class OptionalJobCustomFieldRequest
{
    [Display("Custom field ID")]
    [DataSource(typeof(JobCustomFieldDataHandler))]
    public string? CustomFieldUId { get; set; }
}
