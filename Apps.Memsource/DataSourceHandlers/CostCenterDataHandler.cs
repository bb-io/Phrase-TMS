using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.PhraseTMS.Dtos;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class CostCenterDataHandler(InvocationContext invocationContext) : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
    InvocationContext.AuthenticationCredentialsProviders;

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new PhraseTmsClient(Creds);

        var endpoint = "api2/v1/costCenters";
        var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
        var response = await client.Paginate<CostCenterDto>(request);
         
        return response
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x => new DataSourceItem(x.UId, x.Name));
    }
}