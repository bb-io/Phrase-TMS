using Apps.PhraseTMS.Dtos.Analysis;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Analysis.Responses;

public class GetAnalysisDataResponse : FullAnalysisDto
{
    [Display("Analysis JSON file")]
    public FileReference AnalysisJsonFile { get; }

    public GetAnalysisDataResponse(FullAnalysisDto analysis, FileReference analysisJsonFile)
    {
        Settings = analysis.Settings;
        ImportStatus = analysis.ImportStatus;
        Type = analysis.Type;
        Uid = analysis.Uid;
        CanChangeNetRateScheme = analysis.CanChangeNetRateScheme;
        DateCreated = analysis.DateCreated;
        AnalyseLanguageParts = analysis.AnalyseLanguageParts;
        Outdated = analysis.Outdated;
        Name = analysis.Name;
        AnalysisJsonFile = analysisJsonFile;
    }
}
