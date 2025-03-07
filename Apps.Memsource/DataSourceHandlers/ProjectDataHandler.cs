using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class ProjectDataHandler(InvocationContext invocationContext) : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var request = new RestRequest("/api2/v1/projects", Method.Get);
        if (context.SearchString != null) request.AddQueryParameter("name", context.SearchString);
        var response = await Client.PaginateOnce<ProjectDto>(request);
        return response.OrderByDescending(x => x.DateCreated).Select(x => new DataSourceItem(x.UId, x.Name ?? string.Empty));
    }
}