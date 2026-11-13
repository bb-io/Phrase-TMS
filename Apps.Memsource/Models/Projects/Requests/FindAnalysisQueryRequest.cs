using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class FindAnalysisQueryRequest
    {
        [Display("Name contains")]
        public string? NameContains { get; set; }

        [Display("Target language",
          Description = "Only return analyses containing exclusively this target language")]
        public string? TargetLanguage { get; set; }

        [Display("Most recent")]
        public bool? OnlyMostRecent { get; set; }
    }
}
