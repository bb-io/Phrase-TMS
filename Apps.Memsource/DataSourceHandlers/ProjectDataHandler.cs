using Apps.PhraseTMS.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class ProjectDataHandler(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var actions = new ProjectActions(null!);
        var response = await actions.ListAllProjects(Creds, new() { Name = context.SearchString });

        return response.Projects
            .OrderByDescending(x => x.DateCreated)
            .Select(x => new DataSourceItem(x.UId, x.Name ?? string.Empty));
    }
}