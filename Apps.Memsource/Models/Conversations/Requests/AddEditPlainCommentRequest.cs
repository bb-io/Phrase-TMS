using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class AddEditPlainCommentRequest
{
    [JsonProperty("comment")]
    public string Text { get; set; }
}
