using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class CreateMultipleConversationsRequest
{
    [Display("Segment IDs")]
    public List<string> SegmentIds { get; set; } = [];

    [Display("Conversation texts", Description = "The order and number of items should correspond to the ‘Segment IDs’ input")]
    public List<string> Texts { get; set; } = [];

    [Display("Conversation titles", Description = "The order and number of items should correspond to the ‘Segment IDs’ input")]
    public List<string>? ConversationTitles { get; set; }

    public CreateMultipleConversationsRequest Validate()
    {
        if (SegmentIds.Count != Texts.Count ||
            ConversationTitles != null && ConversationTitles.Count != SegmentIds.Count)
        {
            throw new PluginMisconfigurationException(
                "The order and number of items should correspond to the ‘Segment IDs’ input");
        }

        return this;
    }
}