using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class Comment
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("text")]
    public string? Text { get; set; }

    [JsonProperty("createdBy")]
    [Display("Created by")]
    public UserInfo? CreatedBy { get; set; }

    [JsonProperty("dateCreated")]
    public DateTime? DateCreated { get; set; }

    [JsonProperty("dateModified")]
    [Display("Modified date")]
    public DateTime? DateModified { get; set; }

    [JsonProperty("mentions")]
    public List<Mention> Mentions { get; set; } = new();
}
