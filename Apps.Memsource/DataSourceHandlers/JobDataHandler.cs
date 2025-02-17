using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Dtos;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class JobDataHandler(InvocationContext invocationContext, [ActionParameter] ProjectRequest projectRequest)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private ProjectRequest ProjectRequest { get; set; } = projectRequest;

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(ProjectRequest.ProjectUId))
        {
            throw new PluginMisconfigurationException("Please fill in project first");
        }
        var client = new PhraseTmsClient(Creds);
        var request = new PhraseTmsRequest($"/api2/v2/projects/{ProjectRequest.ProjectUId}/jobs", Method.Get, Creds);
        var jobs = await client.Paginate<JobDto>(request);
        return jobs
            .Where(x => context.SearchString == null ||
                        x.Filename.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x => new DataSourceItem(x.Uid, $"{x.Filename} ({x.InnerId})"));
    }
}