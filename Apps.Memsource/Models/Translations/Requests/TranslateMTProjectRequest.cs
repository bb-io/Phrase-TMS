using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Translations.Requests
{
    public class TranslateMTProjectRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }
        [Display("Job UID")] public string JobUId { get; set; }
        [Display("Source texts")] public IEnumerable<string> SourceTexts { get; set; }
    }
}