
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos
{
    public class LQAAssessmentDto
    {
        [Display("Started date")]
        [JsonProperty("startedDate")]
        public DateTime StartedDate { get; set; }

        [Display("Finished date")]
        [JsonProperty("finishedDate")]
        public DateTime? FinishedDate { get; set; }

        [Display("Assessment result")]
        [JsonProperty("assessmentResult")]
        public AssessmentResult AssessmentResult { get; set; }

        [Display("Overall feedback")]
        [JsonProperty("overallFeedback")]
        public string OverallFeedback { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [Display("Report can be downloaded")]
        [JsonProperty("reportCanBeDownloaded")]
        public bool ReportCanBeDownloaded { get; set; }
    }
    public class AssessmentResult
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [Display("Issue counts")]
        [JsonProperty("issueCounts")]
        public IssueCounts IssueCounts { get; set; }
    }

    public class IssueCounts
    {
        [JsonProperty("critical")]
        public double Critical { get; set; }

        [Display("Critical repeated")]
        [JsonProperty("criticalRepeated")]
        public double CriticalRepeated { get; set; }

        [JsonProperty("major")]
        public double Major { get; set; }

        [Display("Major repeated")]
        [JsonProperty("majorRepeated")]
        public double MajorRepeated { get; set; }

        [JsonProperty("minor")]
        public double Minor { get; set; }

        [Display("Minor repeated")]
        [JsonProperty("minorRepeated")]
        public double MinorRepeated { get; set; }

        [JsonProperty("neutral")]
        public double Neutral { get; set; }

        [Display("Neutral repeated")]
        [JsonProperty("neutralRepeated")]
        public double NeutralRepeated { get; set; }
    }

}
