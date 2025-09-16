using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class ConversationReferencesRequest
{
    [Display("Segment ID")]
    [JsonProperty("segmentId")]
    [DataSource(typeof(SegmentDataHandler))]
    public string SegmentId { get; set; }

    [Display("Conversation title")]
    [JsonProperty("conversationTitle")]
    public string? ConversationTitle { get; set; }
}
