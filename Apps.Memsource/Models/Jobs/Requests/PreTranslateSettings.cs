using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class PreTranslateSettings
    {
        [Display("Propagate repetitions")]
        [JsonProperty("autoPropagateRepetitions")]
        public bool? AutoPropagateRepetitions { get; set; }

        [Display("Confirm repetitions")]
        [JsonProperty("confirmRepetitions")]
        public bool? ConfirmRepetitions { get; set; }

        [Display("Set job status completed")]
        [JsonProperty("setJobStatusCompleted")]
        public bool? SetJobStatusCompleted { get; set; }

        [Display("Completed when confirmed")]
        [JsonProperty("setJobStatusCompletedWhenConfirmed")]
        public bool? SetJobStatusCompletedWhenConfirmed { get; set; }

        [Display("Set project status competed")]
        [JsonProperty("setProjectStatusCompleted")]
        public bool? SetProjectStatusCompleted { get; set; }

        [Display("Overwrite existing translation")]
        [JsonProperty("overwriteExistingTranslations")]
        public bool? OverwriteExistingTranslations { get; set; }
    }
}
