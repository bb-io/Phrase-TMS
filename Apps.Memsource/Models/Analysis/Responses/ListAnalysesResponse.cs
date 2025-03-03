using Apps.PhraseTMS.Dtos.Analysis;

namespace Apps.PhraseTMS.Models.Analysis.Responses;

public class ListAnalysesResponse
{
    public IEnumerable<FullAnalysisDto> Analyses { get; set; }
}