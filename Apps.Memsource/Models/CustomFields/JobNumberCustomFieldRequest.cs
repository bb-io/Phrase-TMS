using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields;

public class JobNumberCustomFieldRequest
{
    [Display("Field ID"), DataSource(typeof(JobCustomFieldNumberDataHandler))]
    public string FieldUId { get; set; } = string.Empty;
}
