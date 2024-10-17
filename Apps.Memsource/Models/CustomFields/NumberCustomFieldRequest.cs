using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields
{
    public class NumberCustomFieldRequest
    {
        [Display("Field ID")]
        [DataSource(typeof(CustomFieldNumberDataHandler))]
        public string FieldUId { get; set; }
    }
}
