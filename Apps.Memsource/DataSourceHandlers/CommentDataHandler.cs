using Apps.PhraseTMS.Models.Conversations.Requests;
using Apps.PhraseTMS.Models.Conversations.Responses;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class CommentDataHandler(
    InvocationContext invocationContext, 
    [ActionParameter] JobRequest job,
    [ActionParameter] ConversationRequest conversation
) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest($"api2/v1/jobs/{job.JobUId}/conversations/plains/{conversation.ConversationUId}", Method.Get);
        if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);
        var response = await Client.ExecuteWithHandling<CommentsResponse>(request);
        return response.Comments.Select(x => new DataSourceItem(x.Id, $"{x.Text} - by {x.CreatedBy.FirstName} {x.CreatedBy.LastName} {x.DateCreated.Value}"));
    }
}
