using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields;

public class JobSelectedOptionRequest
{
    [Display("Selected option ID"), DataSource(typeof(JobCustomFieldOptionDataHandler))]
    public string OptionUId { get; set; } = string.Empty;
}
