using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class TmDataHandler(InvocationContext invocationContext) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest($"/api2/v1/transMemories", Method.Get);
        if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);
        var result = await Client.PaginateOnce<TranslationMemoryDto>(request);
        return result.Select(x => new DataSourceItem(x.UId, x.Name));
    }
}