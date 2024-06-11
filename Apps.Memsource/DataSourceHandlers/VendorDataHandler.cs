using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class VendorDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public VendorDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var client = new PhraseTmsClient(Creds);
        var endpoint = "/api2/v1/vendors";

        if (string.IsNullOrWhiteSpace(context.SearchString))
            endpoint = endpoint.SetQueryParameter("name", context.SearchString);

        var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
        var response = await client.Paginate<VendorDto>(request);

        return response
            .Take(20)
            .ToDictionary(x => x.UId, x => x.Name);
    }
}