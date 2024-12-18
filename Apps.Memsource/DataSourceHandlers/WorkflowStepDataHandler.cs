using Apps.PhraseTMS.Models.WorkflowSteps.Response;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class WorkflowStepDataHandler(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var client = new PhraseTmsClient(Creds);
        var endpoint = "/api2/v1/workflowSteps";

        if (!string.IsNullOrWhiteSpace(context.SearchString))
        {
            endpoint = endpoint.SetQueryParameter("name", context.SearchString);
        }

        var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
        var response = await client.Paginate<WorkflowStepDto>(request);

        return response
            .Select(x => new DataSourceItem(x.Uid,  x.Name));
    }
}