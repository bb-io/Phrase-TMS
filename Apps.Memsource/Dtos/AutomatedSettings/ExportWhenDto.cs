using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.AutomatedSettings;

public class ExportWhenDto
{
    [JsonProperty("exportTrigger")] 
    public string ExportTrigger { get; set; } = string.Empty;

    [JsonProperty("workflowStep")]
    public int? WorkflowStep { get; set; }
}