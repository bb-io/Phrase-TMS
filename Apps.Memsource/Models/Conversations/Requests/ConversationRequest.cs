using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class ConversationRequest
{
    [Display("Conversation ID")]
    [DataSource(typeof(ConversationDataHandler))]
    public string ConversationUId { get; set; }
}
