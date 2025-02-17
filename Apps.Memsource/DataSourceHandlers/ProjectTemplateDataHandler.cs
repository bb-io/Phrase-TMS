using Apps.PhraseTMS.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class ProjectTemplateDataHandler(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;


    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var actions = new ProjectActions(null!);
        var projects = await actions.ListAllProjectTemplates(Creds);

        return projects.Templates
            .Where(x => context.SearchString == null ||
                        x.TemplateName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.DateCreated)
            .Take(20)
            .Select(x => new DataSourceItem(x.UId, x.TemplateName));
    }
}