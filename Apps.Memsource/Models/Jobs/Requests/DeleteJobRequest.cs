using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class DeleteJobRequest : ProjectRequest
    {
        [Display("Job UIDs")]
        public IEnumerable<string> JobsUIds { get; set; }
    }
}
