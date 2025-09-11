using Apps.PhraseTMS.Models.Conversation;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers
{
    public class ConversationDataHandler(InvocationContext invocationContext, [ActionParameter] GetConversationRequest conv) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
    {
        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var request = new RestRequest($"api2/v1/jobs/{conv.JobUId}/conversations/plains", Method.Get);
            if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);
            var response = await Client.ExecuteWithHandling<ConversationsResponse>(request);
            return response.Conversations.Select(x => new DataSourceItem(x.Id, $"{x.Type} - status {x.Status.Name} - by {x.CreatedBy.FirstName} {x.CreatedBy.LastName}"));
        }
    }
}
