using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Conversation
{
    public class EditConversationRequest
    {
        [StaticDataSource(typeof(ConversationStatusDataHandler))]
        public string? Status { get; set; }
    }
}
