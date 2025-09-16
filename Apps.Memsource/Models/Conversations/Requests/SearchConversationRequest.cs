using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class SearchConversationRequest
{
    [Display("Include deleted")]
    public bool? IncludeDeleted { get; set; }

    public DateTime? Since { get; set; }
}
