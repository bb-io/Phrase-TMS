using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Connectors;

public class ConnectorDto
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("localToken")]
    public string LocalToken { get; set; } = string.Empty;
}

public class ConnectorsResponse
{
    [JsonProperty("connectors")]
    public IEnumerable<ConnectorDto> Connectors { get; set; } = [];
}
