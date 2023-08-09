using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests
{
    public class AddIgnoredWarningRequest : ProjectRequest
    {
        [Display("Job UID")] public string JobUId { get; set; }
        [Display("Segment UID")] public string SegmentUId { get; set; }
        [Display("Warning ID")] public string WarningId { get; set; }
    }
}