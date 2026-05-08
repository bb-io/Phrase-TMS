using Apps.PhraseTMS.Models.Conversations.Requests;

namespace Apps.PhraseTMS.Models.Conversations.Responses;

public record CreateMultipleConversationsResponse(List<Conversation> Conversations);