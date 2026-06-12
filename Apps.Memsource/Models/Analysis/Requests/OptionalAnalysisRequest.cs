using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class OptionalAnalysisRequest
{
    [Display("Analysis ID")]
    public string? AnalysisUId { get; set; }
}