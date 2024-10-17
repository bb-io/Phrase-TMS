using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields
{
    public class SingleSelectCustomFieldRequest
    {
        [Display("Field ID")]
        [DataSource(typeof(CustomFieldSingleSelectDataHandler))]
        public string FieldUId { get; set; }
    }
}
