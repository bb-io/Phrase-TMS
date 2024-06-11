using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class EditProjectRequest
{
    [Display("Project name")] public string ProjectName { get; set; }

    [StaticDataSource(typeof(ProjectStatusDataHandler))]
    public string Status { get; set; }
}