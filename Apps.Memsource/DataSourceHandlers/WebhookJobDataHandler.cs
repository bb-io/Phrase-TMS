using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class WebhookJobDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] ProjectOptionalRequest projectRequest)
    : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private ProjectOptionalRequest ProjectRequest { get; } = projectRequest;

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ProjectRequest.ProjectUId))
        {
            return [];
        }

        var request = new RestRequest($"/api2/v2/projects/{ProjectRequest.ProjectUId}/jobs", Method.Get);
        if (!string.IsNullOrWhiteSpace(context.SearchString))
        {
            request.AddQueryParameter("filename", context.SearchString);
        }

        var jobs = await Client.PaginateOnce<ListJobDto>(request);
        return jobs.Select(x => new DataSourceItem(x.Uid, $"{x.Filename} ({x.InnerId})"));
    }
}
