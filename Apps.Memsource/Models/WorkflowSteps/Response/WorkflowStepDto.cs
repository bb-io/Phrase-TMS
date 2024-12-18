using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.WorkflowSteps.Response;
    
public class WorkflowStepDto
{
    [Display("Workflow step ID")]
    public string Id { get; set; } = string.Empty;

    [Display("Lqa enabled")]
    public bool LqaEnabled { get; set; }

    [Display("Workflow step name")]
    public string Name { get; set; } = string.Empty;

    [Display("Abbreviation")]
    public string Abbr { get; set; } = string.Empty;
}