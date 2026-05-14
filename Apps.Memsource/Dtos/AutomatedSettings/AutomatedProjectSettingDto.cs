using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.AutomatedSettings;

public class AutomatedProjectSettingDto
{
    [JsonProperty("id")] 
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")] 
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("active")]
    public bool IsActive { get; set; }

    [JsonProperty("monitoredFolders")] 
    public IEnumerable<MonitoredFolderDto> MonitoredFolders { get; set; } = [];

    [JsonProperty("selectedTargetLangs")]
    public IEnumerable<string> SelectedTargetLanguages { get; set; } = [];

    [JsonProperty("translationExports")]
    public IEnumerable<TranslationExportDto> TranslationExports { get; set; } = [];
}