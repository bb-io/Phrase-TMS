using Newtonsoft.Json;

namespace Apps.PhraseTMS.Webhooks.Models.Requests
{
    public class ProjectDetailsDto
    {
        [JsonProperty("uid")]
        public string Uid { get; set; } = default!;

        [JsonProperty("name")]
        public string Name { get; set; } = default!;

        [JsonProperty("owner")]
        public UserRef? Owner { get; set; }

        [JsonProperty("domain")]
        public NamedRef? Domain { get; set; }

        [JsonProperty("subDomain")]
        public NamedRef? SubDomain { get; set; }

        [JsonProperty("client")]
        public ClientRef? Client { get; set; }
    }

    public class UserRef
    {
        [JsonProperty("uid")] public string? Uid { get; set; }
        [JsonProperty("userName")] public string? UserName { get; set; }
        [JsonProperty("email")] public string? Email { get; set; }
    }

    public class NamedRef
    {
        [JsonProperty("name")] public string? Name { get; set; }
    }

    public class ClientRef
    {
        [JsonProperty("uid")] public string? Uid { get; set; }
        [JsonProperty("name")] public string? Name { get; set; }
    }
}
