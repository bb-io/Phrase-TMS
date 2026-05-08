using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Jobs;

public class WorkflowStepDto
{
    [Display("Workflow step ID")]
    [JsonProperty("id")]
    public string Id { get; set; }
    
    [Display("Workflow step UID")]
    [JsonProperty("uid")]
    public string Uid { get; set; }

    [Display("Name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [Display("Order")]
    [JsonProperty("order")]
    public int Order { get; set; }

    [Display("Workflow level")]
    [JsonProperty("workflowLevel")]
    public int WorkflowLevel { get; set; }
}
