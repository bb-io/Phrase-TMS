using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Responses
{
    public class DeleteAnalysesResponse
    {
        [Display("Deleted analyses IDs")]
        public IEnumerable<string> DeletedAnalysesIds { get; set; } = new List<string>();

        [Display("Errors during removal")]
        public IEnumerable<DeleteAnalysisError> Errors { get; set; } = new List<DeleteAnalysisError>();
    }

    public class DeleteAnalysisError
    {
        [Display("Analysis ID")]
        public string AnalysisId { get; set; } = default!;

        public string Error { get; set; } = default!;
    }
}
