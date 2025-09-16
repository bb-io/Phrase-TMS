using Newtonsoft.Json;
using Apps.PhraseTMS.Models.Conversations.Requests;

namespace Apps.PhraseTMS.Dtos.Conversations;

public class ConversationReferencesRequestDto
{
    [JsonProperty("segmentId")]
    public string SegmentId { get; set; }

    [JsonProperty("conversationTitle")]
    public string? ConversationTitle { get; set; }

    [JsonProperty("transGroupId")]
    public uint TransGroupId => (uint)char.GetNumericValue(SegmentId[^1]);

    public ConversationReferencesRequestDto(ConversationReferencesRequest request)
    {
        SegmentId = request.SegmentId;
        ConversationTitle = request.ConversationTitle;
    }
}
