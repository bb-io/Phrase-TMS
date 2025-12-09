using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Requests
{
    public class DeleteAnalysesRequest
    {
        [Display("Analyses IDs", Description = "Optional. If empty, all analyses will be removed.")]
        public IEnumerable<string>? AnalysesIds { get; set; }
    }
}
