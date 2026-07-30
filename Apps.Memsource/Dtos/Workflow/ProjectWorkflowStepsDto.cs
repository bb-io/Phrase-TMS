using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Workflow;

public class ProjectWorkflowStepsDto
{
    [JsonProperty("projectWorkflowSteps")]
    public List<ProjectWorkflowStepDto> ProjectWorkflowSteps { get; set; } = [];
}