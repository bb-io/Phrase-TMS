using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields
{
    public class MultiSelectCustomFieldRequest
    {
        [Display("Field ID")]
        [DataSource(typeof(CustomFieldMultiSelectDataHandler))]
        public string FieldUId { get; set; }
    }
}
