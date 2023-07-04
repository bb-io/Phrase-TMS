using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Analysis.Responses
{
    public class ListAnalysesResponse
    {
        public IEnumerable<AnalysisDto> Analyses { get; set; }
    }
}
