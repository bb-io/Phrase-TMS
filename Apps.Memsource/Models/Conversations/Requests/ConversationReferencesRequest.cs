using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class ConversationReferencesRequest
{
    [Display("Segment ID")]
    [DataSource(typeof(SegmentDataHandler))]
    public string SegmentId { get; set; }

    [Display("Conversation title")]
    public string? ConversationTitle { get; set; }

    [JsonProperty("transGroupId")]
    public uint TransGroupId => (uint)char.GetNumericValue(SegmentId[^1]);
}
