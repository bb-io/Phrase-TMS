using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class CreateFromTemplateRequest
    {
        public string Name { get; set; }

        [Display("Template UID")]
        public string TemplateUId { get; set; }
    }
}
