using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class ProjectTemplateRequest
    {
        [Display("Project template UID")]
        public string ProjectTemplateUId { get; set; } = default!;
    }
}
