using Apps.PhraseTMS.Models.Conversations.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Conversations.Responses;

public class ConversationResponse : Conversation
{
    [Display("JSON file")]
    public FileReference JsonFile { get; set; } = null!;
}
