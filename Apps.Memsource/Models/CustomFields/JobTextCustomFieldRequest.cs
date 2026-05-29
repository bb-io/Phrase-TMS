using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields;

public class JobTextCustomFieldRequest
{
    [Display("Field ID"), DataSource(typeof(JobCustomFieldTextDataHandler))]
    public string FieldUId { get; set; } = string.Empty;
}
