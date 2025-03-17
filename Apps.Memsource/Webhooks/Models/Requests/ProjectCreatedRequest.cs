using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class ProjectCreatedRequest
{
    [Display("Project name contains")]
    public string? ProjectNameContains { get; set; }

    [Display("Created by name contains")]
    [DataSource(typeof(UserNameDataHandler))]
    public string? CreatedByUsername { get; set; }
}