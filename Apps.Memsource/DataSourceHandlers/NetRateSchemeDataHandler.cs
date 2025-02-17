using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class NetRateSchemeDataHandler(InvocationContext invocationContext) : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new PhraseTmsClient(Creds);
        var endpoint = "/api2/v1/netRateSchemes";

        if (string.IsNullOrWhiteSpace(context.SearchString))
            endpoint = endpoint.SetQueryParameter("name", context.SearchString);

        var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
        var response = await client.Paginate<NetRateSchemeDto>(request);

        return response
            .OrderByDescending(x => x.DateCreated)
            .Take(20)
            .Select(x => new DataSourceItem(x.Id, x.Name));
    }
}