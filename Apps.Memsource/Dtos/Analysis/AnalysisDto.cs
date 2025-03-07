using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos.Analysis;

public class AnalysisDto
{

    [Display("Analysis ID")]
    public string UId { get; set; }

    public string Name { get; set; }
}