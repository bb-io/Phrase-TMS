using Apps.PhraseTMS.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class TmDataHandler(InvocationContext invocationContext) : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var actions = new TranslationMemoryActions(null);
        var response = await actions.ListTranslationMemories(Creds, new());

        return response.Memories
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.DateCreated)
            .Take(20)
            .Select(x => new DataSourceItem(x.UId, x.Name));
    }
}