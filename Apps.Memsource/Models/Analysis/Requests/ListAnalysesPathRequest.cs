using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Requests
{
    public class ListAnalysesPathRequest : ProjectRequest
    {
        [Display("Job UID")] public string JobUId { get; set; }
    }
}