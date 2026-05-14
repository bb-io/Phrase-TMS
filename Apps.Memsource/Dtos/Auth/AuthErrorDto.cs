using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Auth;

public class AuthErrorDto
{
    [JsonProperty("error")]
    public string? Error { get; set; }
}