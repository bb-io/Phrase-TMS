using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class AnalysisDto
{
        
    [Display("Analysis ID")]
    public string UId { get; set; }

    public string Name { get; set; }
}