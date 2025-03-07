using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Apps.PhraseTMS.Dtos.Jobs;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class JobDataHandler(InvocationContext invocationContext, [ActionParameter] ProjectRequest projectRequest)
    : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    private ProjectRequest ProjectRequest { get; set; } = projectRequest;

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(ProjectRequest.ProjectUId))
        {
            throw new PluginMisconfigurationException("Please fill in project first");
        }
        var request = new RestRequest($"/api2/v2/projects/{ProjectRequest.ProjectUId}/jobs", Method.Get);
        if (context.SearchString != null) request.AddQueryParameter("filename", context.SearchString);
        var jobs = await Client.PaginateOnce<ListJobDto>(request);
        return jobs.Select(x => new DataSourceItem(x.Uid, $"{x.Filename} ({x.InnerId})"));
    }
}