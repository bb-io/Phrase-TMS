using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Translations.Requests
{
    public class TranslateMTProjectRequest : ProjectRequest
    {
        [Display("Job UID")] public string JobUId { get; set; }
        [Display("Source texts")] public IEnumerable<string> SourceTexts { get; set; }
    }
}