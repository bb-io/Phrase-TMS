using Apps.PhraseTMS.Models.Languages.Response;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class LanguageDataHandler(InvocationContext invocationContext) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest("/api2/v1/languages", Method.Get);
        request.AddQueryParameter("active", "true");
        var response = await Client.ExecuteWithHandling<LanguagesResponse>(request);

        return response.Languages
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x=> new DataSourceItem(x.Code,x.Name));
    }
}