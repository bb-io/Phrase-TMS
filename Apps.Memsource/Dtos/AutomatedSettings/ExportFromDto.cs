using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.AutomatedSettings;

public class ExportFromDto
{
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    [JsonProperty("workflowStep")]
    public int? WorkflowStep { get; set; }
}