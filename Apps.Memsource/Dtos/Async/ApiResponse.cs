
using System.Text.Json.Serialization;

namespace Apps.PhraseTMS.Dtos.Async;

public class ApiResponse
{

    [JsonPropertyName("updated")]
    public int? Updated { get; set; }

    [JsonPropertyName("errors")]
    public List<string>? Errors { get; set; } = new();
    
}
