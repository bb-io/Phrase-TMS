using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields;

public class JobSelectedOptionsRequest
{
    [Display("Selected option IDs"), DataSource(typeof(JobCustomFieldMultiSelectOptionDataHandler))]
    public IEnumerable<string> OptionUIds { get; set; } = Array.Empty<string>();
}
