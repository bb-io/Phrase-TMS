using Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class EditProjectRequest
{
    [Display("Project name")] public string ProjectName { get; set; }

    [DataSource(typeof(ProjectStatusDataHandler))]
    public string Status { get; set; }
}