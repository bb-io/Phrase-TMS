using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields
{
    public class SelectedOptionRequest
    {
        [Display("Selected option ID")]
        [DataSource(typeof(CustomFieldOptionDataHandler))]
        public string OptionUId { get; set; }
    }
}
