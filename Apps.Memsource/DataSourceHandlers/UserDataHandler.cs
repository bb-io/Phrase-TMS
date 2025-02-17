using Apps.PhraseTMS.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class UserDataHandler(InvocationContext invocationContext) : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var actions = new UserActions();
        var response = await actions.ListAllUsers(Creds, new());

        return response.Users
            .Where(x => context.SearchString == null ||
                        x.FullName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x=> new DataSourceItem(x.UId, x.FullName));
    }
}