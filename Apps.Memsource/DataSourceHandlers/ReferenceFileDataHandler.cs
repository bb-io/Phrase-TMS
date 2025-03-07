using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class ReferenceFileDataHandler : PhraseInvocable, IAsyncDataSourceItemHandler
{
    private readonly ReferenceFileRequest _referenceFileRequest;

    public ReferenceFileDataHandler(InvocationContext invocationContext, [ActionParameter] ReferenceFileRequest referenceFileRequest) : base(
        invocationContext)
    {
        _referenceFileRequest = referenceFileRequest;
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_referenceFileRequest.ProjectUId))
        {
            throw new PluginMisconfigurationException("Please fill in project first");
        }

        var request = new RestRequest($"/api2/v1/projects/{_referenceFileRequest.ProjectUId}/references?filename={context.SearchString}", Method.Get);
        var referenceFiles = await Client.Paginate<ReferenceFileInfoDto>(request);
        return referenceFiles.Select(x => new DataSourceItem(x.UId, x.Filename));
    }
}