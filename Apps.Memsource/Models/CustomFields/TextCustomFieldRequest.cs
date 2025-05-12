using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields;

public class TextCustomFieldRequest
{
    [Display("Field ID"), DataSource(typeof(CustomFieldTextDataHandler))]
    public string FieldUId { get; set; } = string.Empty;
}
