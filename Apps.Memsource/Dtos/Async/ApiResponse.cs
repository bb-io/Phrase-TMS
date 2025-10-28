
using System.Text.Json.Serialization;

namespace Apps.PhraseTMS.Dtos.Async;

public class ApiResponse
{
    [JsonPropertyName("updated")]
    public int? Updated { get; set; }

    [JsonPropertyName("errors")]
    public List<ApiError>? Errors { get; set; } = new();
}

public class ApiError
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("args")]
    public ErrorArgs Args { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }
}

public class ErrorArgs
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("uids")]
    public List<string> Uids { get; set; } = new();
}