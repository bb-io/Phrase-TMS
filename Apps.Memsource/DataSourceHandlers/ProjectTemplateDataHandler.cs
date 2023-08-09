using Apps.PhraseTMS.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class ProjectTemplateDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;
    
    public ProjectTemplateDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var actions = new ProjectActions();
        var projects = await actions.ListAllProjectTemplates(Creds);
        
        return projects.Templates
            .Where(x => context.SearchString == null ||
                        x.TemplateName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.DateCreated)
            .Take(20)
            .ToDictionary(x => x.UId, x => x.TemplateName);
    }
}