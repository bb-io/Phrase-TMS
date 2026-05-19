using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Analysis.Responses;

public record ExportProjectAnalysisResponse(FileReference ExportedAnalysis)
{
    [Display("Analysis file")] 
    public FileReference ExportedAnalysis { get; set; } = ExportedAnalysis;
}