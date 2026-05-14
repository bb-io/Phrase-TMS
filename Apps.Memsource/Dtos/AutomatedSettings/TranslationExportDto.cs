using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.AutomatedSettings;

public class TranslationExportDto
{
    [JsonProperty("name")] 
    public string Name { get; set; } = string.Empty;

    [JsonProperty("exportFrom")]
    public ExportFromDto? ExportFrom { get; set; }

    [JsonProperty("exportWhen")]
    public ExportWhenDto? ExportWhen { get; set; }

    [JsonProperty("exportedWorkflowStepToDeliveredAfterExport")]
    public bool? DeliveredAfterExport { get; set; }
}