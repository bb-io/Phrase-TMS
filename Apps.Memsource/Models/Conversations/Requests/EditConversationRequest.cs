using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class EditConversationRequest
{
    [StaticDataSource(typeof(ConversationStatusDataHandler))]
    public string? Status { get; set; }
}
