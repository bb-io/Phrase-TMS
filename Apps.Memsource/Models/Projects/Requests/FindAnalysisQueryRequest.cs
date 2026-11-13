using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class FindAnalysisQueryRequest
    {
        [Display("Name contains")]
        public string? NameContains { get; set; }

        [Display("Target language",
          Description = "Only return analyses containing exclusively this target language")]
        [DataSource(typeof(LanguageDataHandler))]
        public string? TargetLanguage { get; set; }

        [Display("Most recent")]
        public bool? OnlyMostRecent { get; set; }
    }
}
