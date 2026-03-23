using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class StartLqaAssessmentResponse
{
    [Display("LQA profile")]
    [JsonProperty("lqaProfile")]
    public LqaProfileDto? LqaProfile { get; set; }

    [Display("Started date")]
    [JsonProperty("startedDate")]
    public DateTime? StartedDate { get; set; }
}
