using Apps.PhraseTMS.Models.Conversations.Responses;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class SegmentDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] JobRequest job,
    [ActionParameter] ProjectRequest project
) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest($"/api2/v1/projects/{project.ProjectUId}/jobs/{job.JobUId}/segments", Method.Get);
        if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);
        request.AddQueryParameter("endIndex", 500000);
        var response = await Client.ExecuteWithHandling<SegmentsResponse>(request);
        return response.Segments.Select(x => new DataSourceItem(x.Id, x.Source));
    }
}
