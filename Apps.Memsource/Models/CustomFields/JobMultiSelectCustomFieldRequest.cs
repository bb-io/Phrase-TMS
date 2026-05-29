using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields;

public class JobMultiSelectCustomFieldRequest
{
    [Display("Field ID"), DataSource(typeof(JobCustomFieldMultiSelectDataHandler))]
    public string FieldUId { get; set; } = string.Empty;
}
