using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class EditProjectTransMemoriesRequest
    {
        [Display("Target language")]
        [DataSource(typeof(LanguageDataHandler))]
        public string TargetLanguage { get; set; }

        [Display("Workflow step ID")]
        [DataSource(typeof(WorkflowStepDataHandler))]
        public string? WorkflowStepUid { get; set; }

        [Display("Enable custom TM order?")]
        public bool? OrderEnabled { get; set; }

        [Display("Trans memory IDs")]
        [DataSource(typeof(TmDataHandler))]
        public string[] TransMemoryUids { get; set; }

        [Display("Read modes")]
        public bool[]? ReadModes { get; set; }

        [Display("Write modes")]
        public bool[]? WriteModes { get; set; }

        [Display("Penalties 0–100")]
        public int[]? Penalties { get; set; }

        [Display("Apply 101%-only penalties")]
        public bool[]? ApplyPenaltyTo101Only { get; set; }

        [Display("Order")]
        public int[]? Orders { get; set; }
    }
}
