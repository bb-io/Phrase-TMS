using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class AddTargetLanguageRequest : ProjectRequest
{
    [Display("Target languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string> TargetLanguages { get; set; }
}