using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos
{
    public class UserPretranslationDto
    {
        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("userName")]
        public string? UserName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("role")]
        public string? Role { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("uid")]
        public string? Uid { get; set; }
    }
}
