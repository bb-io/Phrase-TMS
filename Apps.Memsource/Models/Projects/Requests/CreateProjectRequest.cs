using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }

        [Display("Source language")]
        public string SourceLanguage { get; set; }

        [Display("Target languages")]
        public IEnumerable<string> TargetLanguages { get; set; }
    }
}
