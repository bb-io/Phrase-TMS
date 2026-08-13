using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Connectors;

public class ConnectorFilesResponse
{
    [JsonProperty("files")]
    public IEnumerable<ConnectorFileDto> Files { get; set; } = [];

    [JsonProperty("encodedCurrentFolder")]
    public string? EncodedCurrentFolder { get; set; }

    [JsonProperty("currentFolder")]
    public string? CurrentFolder { get; set; }
}

public class ConnectorFileDto
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("encodedName")]
    public string EncodedName { get; set; } = string.Empty;

    [JsonProperty("directory")]
    public bool IsDirectory { get; set; }
}
