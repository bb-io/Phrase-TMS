using Apps.PhraseTMS.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class FileNameDataHandler(InvocationContext invocationContext) : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;
    
    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var actions = new FileActions(null);
        var response = await actions.ListAllFiles(Creds, new());

        return response.Files
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x => new DataSourceItem(x.Name, x.Name));
    }
}