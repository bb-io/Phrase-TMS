using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class ExportJobsResponse
    {
        public IEnumerable<JobReference> Jobs { get; set; }
    }
    public class JobReference
    {
        [Display("Job UID")]
        public string Uid { get; set; }
    }
}
