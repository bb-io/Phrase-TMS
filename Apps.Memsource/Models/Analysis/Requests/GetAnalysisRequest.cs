using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Requests
{
    public class GetAnalysisRequest
    {
        [Display("Analysis UID")]
        public string AnalysisUId { get; set; }
    }
}
