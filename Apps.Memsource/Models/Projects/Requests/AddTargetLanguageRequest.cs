using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class AddTargetLanguageRequest : ProjectRequest
    {
        [Display("Target languages")] public IEnumerable<string> TargetLanguages { get; set; }
    }
}