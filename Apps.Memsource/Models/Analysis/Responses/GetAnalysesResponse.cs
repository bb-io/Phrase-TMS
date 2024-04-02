using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Analysis.Responses;

public class GetAnalysesResponse
{
    public List<AnalysisDto> Content { get; set; }
}