using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields;

public class JobUrlCustomFieldRequest
{
    [Display("Field ID"), DataSource(typeof(JobCustomFieldUrlDataHandler))]
    public string FieldUId { get; set; } = string.Empty;
}
