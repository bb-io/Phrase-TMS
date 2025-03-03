
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class CustomFieldDateDataHandler(InvocationContext invocationContext) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest("api2/v1/customFields", Method.Get);
        request.AddQueryParameter("types", "DATE");
        if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);        
        var response = await Client.PaginateOnce<CustomFieldDto>(request);
        return response.Select(x => new DataSourceItem(x.UId, x.Name));
    }
}

