using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class SetProjectTermBasesRequest
    {
        [Display("Read term base IDs")]
        [DataSource(typeof(TermBaseDataHandler))]
        public IEnumerable<string>? ReadTermBaseIds { get; set; }

        [Display("Write term base ID")]
        [DataSource(typeof(TermBaseDataHandler))]
        public string? WriteTermBaseId { get; set; }

        [Display("QA term base IDs")]
        [DataSource(typeof(TermBaseDataHandler))]
        public IEnumerable<string>? QualityAssuranceTermBaseIds { get; set; }

        [Display("Target language")]
        [DataSource(typeof(LanguageDataHandler))]
        public string? TargetLang { get; set; }
    }
}
