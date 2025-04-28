using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class RunQAResponse

{
    [Display("Segment warnings")]
    [JsonProperty("segmentWarnings")]
    public List<SegmentWarningDto>? SegmentWarnings { get; set; } = new();

    
    [Display("Outstanding warnings")]
    [JsonIgnore]
    public bool OutstandingWarnings { get; set; }
}