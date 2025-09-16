using Newtonsoft.Json;
using Apps.PhraseTMS.Models.Conversations.Requests;

namespace Apps.PhraseTMS.Dtos.Conversations;

public class CreateConversationDto
{
    [JsonProperty("references")]
    public ConversationReferencesRequestDto References { get; set; }

    [JsonProperty("comment")]
    public AddEditPlainCommentRequest Comment { get; set; }
}
