using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields
{
    public class UrlCustomFieldRequest
    {
        [Display("Field UID")]
        [DataSource(typeof(CustomFieldUrlDataHandler))]
        public string FieldUId { get; set; }
    }
}
