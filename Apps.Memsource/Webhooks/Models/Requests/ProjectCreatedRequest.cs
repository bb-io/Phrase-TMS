using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class ProjectCreatedRequest
{
    [Display("Project name contains")]
    public string? ProjectNameContains { get; set; }

    [Display("Created by name contains")]
    public string? CreatedByUsername { get; set; }
}