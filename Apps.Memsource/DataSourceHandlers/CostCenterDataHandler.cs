using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Dtos;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class CostCenterDataHandler(InvocationContext invocationContext) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest("api2/v1/costCenters", Method.Get);
        if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);
        var response = await Client.PaginateOnce<CostCenterDto>(request);         
        return response.Select(x => new DataSourceItem(x.UId, x.Name));
    }
}