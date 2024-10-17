
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class CustomFieldSingleSelectDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public CustomFieldSingleSelectDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new PhraseTmsClient(Creds);

        var endpoint = "api2/v1/customFields";
        var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
        var response = await client.Paginate<CustomFieldDto>(request);
        return response
            .Where(y => y.type == "SINGLE_SELECT")
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToDictionary(x => x.UId, x => x.Name);
    }
}

