using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields;

public class JobSingleSelectCustomFieldRequest
{
    [Display("Field ID"), DataSource(typeof(JobCustomFieldSingleSelectDataHandler))]
    public string FieldUId { get; set; } = string.Empty;
}
