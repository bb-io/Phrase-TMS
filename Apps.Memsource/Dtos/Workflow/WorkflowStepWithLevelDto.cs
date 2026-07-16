using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Workflow;

public class WorkflowStepWithLevelDto : WorkflowStepDto
{
    [Display("Workflow level")]
    [JsonProperty("workflowLevel")]
    public int WorkflowLevel { get; set; }
}