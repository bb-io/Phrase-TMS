using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class AddTargetLanguageRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }
        [Display("Target languages")] public IEnumerable<string> TargetLanguages { get; set; }
    }
}