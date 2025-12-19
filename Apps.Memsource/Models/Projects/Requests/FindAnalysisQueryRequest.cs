using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class FindAnalysisQueryRequest
    {
        [Display("Name contains")]
        public string? NameContains { get; set; }

        //[Display("Type")]
        //public string? Type { get; set; }

        [Display("Most recent")]
        public bool? OnlyMostRecent { get; set; }
    }
}
