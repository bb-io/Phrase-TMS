using Apps.PhraseTMS.Models.Languages.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class LanguageDataHandler(InvocationContext invocationContext) :  BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new PhraseTmsClient(Creds);
        var request = new PhraseTmsRequest("/api2/v1/languages", Method.Get, Creds);

        var response = await client.ExecuteWithHandling<LanguagesResponse>(request);

        return response.Languages
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x=> new DataSourceItem(x.Code,x.Name));
    }
}