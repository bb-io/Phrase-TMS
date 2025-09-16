using Apps.PhraseTMS.Models.Conversations.Responses;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class ConversationDataHandler(InvocationContext invocationContext, [ActionParameter] JobRequest conv) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest($"api2/v1/jobs/{conv.JobUId}/conversations/plains", Method.Get);
        if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);
        var response = await Client.ExecuteWithHandling<ConversationsResponse>(request);
        return response.Conversations.Select(x => new DataSourceItem(x.Id, $"{x.Comments[0].Text} - status {x.Status.Name} - by {x.CreatedBy.FirstName} {x.CreatedBy.LastName} - created {x.Comments[0].DateCreated}"));
    }
}
