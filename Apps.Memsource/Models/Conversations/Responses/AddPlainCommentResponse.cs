using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common;
using Apps.PhraseTMS.Models.Conversations.Requests;

namespace Apps.PhraseTMS.Models.Conversations.Responses;

public class AddPlainCommentResponse
{
    [JsonProperty("id")]
    [Display("Comment ID")]
    public string Id { get; set; }

    [JsonProperty("conversation")]
    public Conversation Conversation { get; set; }
}
