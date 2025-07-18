using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests;
public class OptionalTargetLanguageRequest
{
    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? TargetLang { get; set; }
}
