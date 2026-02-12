using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class SetProjectTranslationMemoriesRequest
    {
        [Display("Translation memory UIDs")]
        [DataSource(typeof(TmDataHandler))]
        public IEnumerable<string> TranslationMemoryUids { get; set; } = Array.Empty<string>();

        [Display("Target language")]
        [DataSource(typeof(LanguageDataHandler))]
        public string? TargetLang { get; set; }

        [Display("Workflow step UID")]
        [DataSource(typeof(WorkflowStepDataHandler))]
        public string? WorkflowStepUid { get; set; }

        [Display("Read mode")]
        public bool? ReadMode { get; set; }

        [Display("Write mode")]
        public bool? WriteMode { get; set; }

        [Display("Penalty")]
        public int? Penalty { get; set; }

        [Display("Apply penalty to 101% only")]
        public bool? ApplyPenaltyTo101Only { get; set; }

        [Display("Order enabled")]
        public bool? OrderEnabled { get; set; }

        [Display("Order")]
        public int[]? Order { get; set; }
    }
}
