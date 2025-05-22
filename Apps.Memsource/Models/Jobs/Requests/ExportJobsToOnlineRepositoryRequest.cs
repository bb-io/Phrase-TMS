using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class ExportJobsToOnlineRepositoryRequest
    {
        [Display("Job IDs")]
        [DataSource(typeof(JobDataHandler))]
        public IEnumerable<string> JobIds { get; set; }
    }
}
