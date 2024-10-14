using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class ListAnalysesQueryRequest
{
    public string? Name { get; set; }
    [Display("Analysis ID")] public string? Uid { get; set; }
    public string? Sort { get; set; }
    [Display("Only owner org")] public bool? OnlyOwnerOrg { get; set; }
}