using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class GetProjectRequest
    {
        [Display("Project UID")]
        public string ProjectUId { get; set; }
    }
}
