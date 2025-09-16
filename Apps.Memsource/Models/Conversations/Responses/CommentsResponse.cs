using Newtonsoft.Json;
using Apps.PhraseTMS.Models.Conversations.Requests;

namespace Apps.PhraseTMS.Models.Conversations.Responses;

public class CommentsResponse
{
    [JsonProperty("comments")]
    public List<Comment> Comments { get; set; }
}
