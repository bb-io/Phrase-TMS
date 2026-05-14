using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.AutomatedSettings;

public class MonitoredFolderDto
{
    [JsonProperty("folderNames")]
    public IEnumerable<string> FolderNames { get; set; } = [];

    [JsonProperty("humanReadableFolderPath")]
    public string HumanReadableFolderPath { get; set; } = string.Empty;

    [JsonProperty("localToken")]
    public string LocalToken { get; set; } = string.Empty;

    [JsonProperty("projectTemplateUid")] 
    public string ProjectTemplateUid { get; set; } = string.Empty;

    [JsonProperty("remoteFolder")]
    public string RemoteFolder { get; set; } = string.Empty;

    [JsonProperty("fileNameRegex")]
    public string? FileNameRegex { get; set; }

    [JsonProperty("includeSubfolders")]
    public bool? IncludeSubfolders { get; set; }

    [JsonProperty("moveToProcessedSubfolder")]
    public bool? MoveToProcessedSubfolder { get; set; }

    [JsonProperty("processedSubfolder")]
    public string? ProcessedSubfolder { get; set; }

    [JsonProperty("selectedMonitoredFiles")]
    public IEnumerable<SelectedMonitoredFileDto>? SelectedMonitoredFiles { get; set; }
}