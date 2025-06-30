using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class PreTranslateSettings
    {
        [Display("Propagate repetitions", Description = "Propagate repetitions. Default: false")]
        [JsonProperty("autoPropagateRepetitions")]
        public bool? AutoPropagateRepetitions { get; set; }

        [Display("Confirm repetitions", Description = "Set segment status to confirmed for: Repetitions. Default: false")]
        [JsonProperty("confirmRepetitions")]
        public bool? ConfirmRepetitions { get; set; }

        [Display("Set job status completed", Description = "Pre-translate and set job to completed: Set job to completed once pre-translated. Default: false")]
        [JsonProperty("setJobStatusCompleted")]
        public bool? SetJobStatusCompleted { get; set; }

        [Display("Completed when confirmed", Description = "Pre-translate and set job to completed when all segments confirmed: Set job to completed once pre-translated and all segments are confirmed. Default: false")]
        [JsonProperty("setJobStatusCompletedWhenConfirmed")]
        public bool? SetJobStatusCompletedWhenConfirmed { get; set; }

        [Display("Set project status competed", Description = "Pre-translate & set job to completed: Set project to completed once all jobs pre-translated. Default: false")]
        [JsonProperty("setProjectStatusCompleted")]
        public bool? SetProjectStatusCompleted { get; set; }

        [Display("Overwrite existing translation", Description = "Overwrite existing translations in target segments. Default: false")]
        [JsonProperty("overwriteExistingTranslations")]
        public bool? OverwriteExistingTranslations { get; set; }

        // translation memory settings
        [Display("Use translation memory")]
        [JsonProperty("useTranslationMemory")]
        public bool? UseTranslationMemory { get; set; }

        [Display("Translation memory threshold", Description = "Pre-translation threshold percent. Default: 0.7")]
        [JsonProperty("translationMemoryThreshold")]
        public float? TranslationMemoryThreshold { get; set; }

        [Display("Confirm 100 percent matches (TM)")]
        [JsonProperty("confirm100PercentMatches")]
        public bool? Confirm100PercentMatches { get; set; }

        [Display("Confirm 101 percent matches (TM)")]
        [JsonProperty("confirm101PercentMatches")]
        public bool? Confirm101PercentMatches { get; set; }

        [Display("Lock 100 percent matches (TM)")]
        [JsonProperty("lock100PercentMatchesTM")]
        public bool? Lock100PercentMatchesTM { get; set; }

        [Display("Lock 101 percent matches (TM)")]
        [JsonProperty("lock101PercentMatches")]
        public bool? Lock101PercentMatches { get; set; }

        // machine translation settings
        [Display("Use machine translation")]
        [JsonProperty("useMachineTranslation")]
        public bool? UseMachineTranslation { get; set; }

        [Display("Machine translation behavior")]
        [StaticDataSource(typeof(MachineTranslationBehaviorSource))]
        [JsonProperty("machineTranslationBehavior")]
        public string? MachineTranslationBehavior { get; set; }

        [Display("Lock 100 percent matches (MT)")]
        [JsonProperty("lock100PercentMatchesMT")]
        public bool? Lock100PercentMatchesMT { get; set; }

        [Display("Confirm matches (MT)")]
        [JsonProperty("confirmMatches")]
        public bool? ConfirmMatches { get; set; }

        [Display("Confirm matches threshold (MT)", Description = "Machine translation suggestions percent. Default: 1.0. Required range: 0 < x < 1")]
        [JsonProperty("confirmMatchesThreshold")]
        public float? ConfirmMatchesThreshold { get; set; }

        [Display("Use alt translations only (MT)", Description = "Do not put machine translations to target and use alt-trans fields (alt-trans in mxlf). Default: false")]
        [JsonProperty("useAltTransOnly")]
        public bool? UseAltTransOnly { get; set; }

        [Display("MT suggest only TM below", Description = "Suggest MT only for segments with a TM match below. Default: true")]
        [JsonProperty("mtSuggestOnlyTmBelow")]
        public bool? MtSuggestOnlyTmBelow { get; set; }

        [Display("MT suggest only TM below threshold", Description = "Suggest MT only for segments with a TM match below threshold. Default: 1.0. Required range: 0 < x < 1.01")]
        [JsonProperty("mtSuggestOnlyTmBelowThreshold")]
        public float? MtSuggestOnlyTmBelowThreshold { get; set; }

        // non-translatable dettings
        [Display("Pre-translate non-translatables")]
        [JsonProperty("preTranslateNonTranslatables")]
        public bool? PreTranslateNonTranslatables { get; set; }

        [Display("Confirm 100 percent matches (NT)")]
        [JsonProperty("confirm100PercentMatchesNT")]
        public bool? Confirm100PercentMatchesNT { get; set; }

        [Display("Lock 100 percent matches (NT)")]
        [JsonProperty("lock100PercentMatchesNT")]
        public bool? Lock100PercentMatchesNT { get; set; }
    }
}
