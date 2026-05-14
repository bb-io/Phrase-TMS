using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.AutomatedSettings;

public class SelectedMonitoredFileDto
{
    [JsonProperty("encodedFileName")]
    public string EncodedFileName { get; set; } = string.Empty;

    [JsonProperty("fileName")]
    public string FileName { get; set; } = string.Empty;

    [JsonProperty("uniqueIdentifier")]
    public string? UniqueIdentifier { get; set; }
}