using Apps.PhraseTMS.Dtos.Connectors;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class ConnectorDataHandler(InvocationContext invocationContext)
    : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(
        DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var request = new RestRequest("/api2/v1/connectors", Method.Get);
        var response = await Client.ExecuteWithHandling<ConnectorsResponse>(request);

        return response.Connectors
            .Where(x => !string.IsNullOrWhiteSpace(x.LocalToken))
            .Where(x => string.IsNullOrWhiteSpace(context.SearchString)
                || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.Name)
            .Select(x => new DataSourceItem(x.LocalToken, x.Name));
    }
}
