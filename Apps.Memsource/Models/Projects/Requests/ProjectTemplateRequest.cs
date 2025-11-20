using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class ProjectTemplateRequest
    {
        [Display("Project template UID")]
        [DataSource(typeof(ProjectTemplateDataHandler))]
        public string ProjectTemplateUId { get; set; } = default!;
    }
}
