using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Requests
{
    public class ListAnalysesPathRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }
        [Display("Job UID")] public string JobUId { get; set; }
    }
}