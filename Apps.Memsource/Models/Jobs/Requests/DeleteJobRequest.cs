using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class DeleteJobRequest
    {
        [Display("Project UID")]
        public string ProjectUId { get; set; }

        [Display("Job UIDs")]
        public IEnumerable<string> JobsUIds { get; set; }
    }
}
