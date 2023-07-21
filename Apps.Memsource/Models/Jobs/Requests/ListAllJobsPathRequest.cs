using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class ListAllJobsPathRequest
    {
        [Display("Project UID")]
        public string ProjectUId { get; set; }
    }
}
