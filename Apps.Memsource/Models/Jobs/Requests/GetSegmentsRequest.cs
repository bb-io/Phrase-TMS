using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class GetSegmentsRequest
    {
        [Display("Project UID")]
        public string ProjectUId { get; set; }

        [Display("Job UID")]
        public string JobUId { get; set; }
    }
}
