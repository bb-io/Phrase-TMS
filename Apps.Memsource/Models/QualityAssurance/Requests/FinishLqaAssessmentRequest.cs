using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests;

public class FinishLqaAssessmentRequest
{
    [Display("Overall feedback")]
    [JsonProperty("overallFeedback")]
    public string? OverallFeedback { get; set; }
}
