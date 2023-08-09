using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class ProjectRequest
{
    [Display("Project")]
    [DataSource(typeof(ProjectDataHandler))]
    public string ProjectUId { get; set; }
}