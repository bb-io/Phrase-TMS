using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class EditJobPath : ProjectRequest
    {
        [Display("Job UID")] public string JobUId { get; set; }
    }
}