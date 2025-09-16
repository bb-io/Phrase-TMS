using Newtonsoft.Json;
using Apps.PhraseTMS.Models.Conversations.Requests;

namespace Apps.PhraseTMS.Models.Conversations.Responses;

public class ConversationsResponse
{
    [JsonProperty("conversations")]
    public List<Conversation> Conversations { get; set; }
}
