using Apps.PhraseTMS.Models.Conversations.Requests;
using Apps.PhraseTMS.Models.Conversations.Responses;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using System.Globalization;

namespace Apps.PhraseTMS.Actions;

[ActionList("Conversations")]
public class ConversationActions(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{

    [Action("Get conversation", Description = "Gets plain conversation")]
    public async Task<Conversation> GetConversation([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ConversationRequest conv)
    {
        var endpoint = $"/api2/v1/jobs/{jobRequest.JobUId}/conversations/plains/{conv.ConversationUId}";
        var request = new RestRequest(endpoint, Method.Get);
        var response = await Client.ExecuteWithHandling<Conversation>(request);

        return response;
    }

    [Action("Search conversations", Description = "Searches conversations")]
    public async Task<ConversationsResponse> SearchConversations([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest, [ActionParameter] SearchConversationRequest input)
    {
        var endpoint = $"/api2/v1/jobs/{jobRequest.JobUId}/conversations/plains";
        var request = new RestRequest(endpoint, Method.Get);
        if (input.IncludeDeleted.HasValue)
            request.AddQueryParameter("includeDeleted",
                input.IncludeDeleted.Value ? "true" : "false");

        if (input.Since.HasValue)
        {
            var sinceUtc = input.Since.Value.ToUniversalTime();
            var sinceStr = sinceUtc.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
            request.AddQueryParameter("since", sinceStr);
        }

        var response = await Client.ExecuteWithHandling<ConversationsResponse>(request);

        return response;
    }


    [Action("Delete conversation", Description = "Deletes plain conversation")]
    public async Task DeleteConversation([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ConversationRequest conv)
    {
        var endpoint = $"/api2/v1/jobs/{jobRequest.JobUId}/conversations/plains/{conv.ConversationUId}";
        var request = new RestRequest(endpoint, Method.Delete);
        var response = await Client.ExecuteWithHandling(request);
    }

    [Action("Edit conversation", Description = "Edits plain conversation")]
    public async Task<Conversation> EditConversation([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ConversationRequest conv,
        [ActionParameter] EditConversationRequest input)
    {
        var endpoint = $"/api2/v1/jobs/{jobRequest.JobUId}/conversations/plains/{conv.ConversationUId}";
        var request = new RestRequest(endpoint, Method.Put);

        if (!string.IsNullOrEmpty(input.Status))
        {
            request.AddJsonBody(new {status = input.Status});
        }

        var response = await Client.ExecuteWithHandling<Conversation>(request);

        return response;
    }

    [Action("Add conversation", Description = "Adds plain conversation")]
    public async Task<Conversation> AddConversation([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ConversationReferencesRequest references,
        [ActionParameter] AddEditPlainCommentRequest comment)
    {
        var endpoint = $"/api2/v3/jobs/{jobRequest.JobUId}/conversations/plains";
        var request = new RestRequest(endpoint, Method.Post);

        var body = new Dictionary<string, object?>
        {
            { "references", references },
            { "comment", comment }
        };
        request.AddJsonBody(body);

        var response = await Client.ExecuteWithHandling<Conversation>(request);
        return response;
    }

    [Action("Add plain comment", Description = "Adds plain comment")]
    public async Task<AddPlainCommentResponse> AddPlainComment([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ConversationRequest conv,
        [ActionParameter] AddEditPlainCommentRequest input)
    {
        var endpoint = $"/api2/v3/jobs/{jobRequest.JobUId}/conversations/plains/{conv.ConversationUId}/comments";
        var request = new RestRequest(endpoint, Method.Post);
        request.AddJsonBody(input);
        
        var response = await Client.ExecuteWithHandling<AddPlainCommentResponse>(request);
        return response;
    }

    [Action("Edit plain comment", Description = "Edits plain comment")]
    public async Task<Conversation> EditPlainComment([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ConversationRequest conv,
        [ActionParameter] CommentRequest commentRequest,
        [ActionParameter] AddEditPlainCommentRequest input)
    {
        var endpoint = $"/api2/v3/jobs/{jobRequest.JobUId}/conversations/plains/{conv.ConversationUId}/comments/{commentRequest.CommentUId}";
        var request = new RestRequest(endpoint, Method.Put);
        request.AddJsonBody(input);

        var response = await Client.ExecuteWithHandling<Conversation>(request);
        return response;
    }

    [Action("Delete plain comment", Description = "Deletes plain comment")]
    public async Task DeletePlainComment([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ConversationRequest conv,
        [ActionParameter] CommentRequest commentRequest)
    {
        var endpoint = $"/api2/v1/jobs/{jobRequest.JobUId}/conversations/plains/{conv.ConversationUId}/comments/{commentRequest.CommentUId}";
        var request = new RestRequest(endpoint, Method.Delete);

        var response = await Client.ExecuteWithHandling(request);
    }
}
