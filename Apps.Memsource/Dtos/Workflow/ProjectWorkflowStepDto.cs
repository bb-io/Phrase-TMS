using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Workflow;

public class ProjectWorkflowStepDto
{
    [JsonProperty("workflowLevel")] 
    public int WorkflowLevel { get; set; }
    
    [JsonProperty("workflowStep")] 
    public WorkflowStepDto? WorkflowStep { get; set; }
}