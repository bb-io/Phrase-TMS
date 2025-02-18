using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class ReferenceFileDataHandler : BaseInvocable, IAsyncDataSourceItemHandler
{
    private readonly ReferenceFileRequest _referenceFileRequest;

    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public ReferenceFileDataHandler(InvocationContext invocationContext, [ActionParameter] ReferenceFileRequest referenceFileRequest) : base(
        invocationContext)
    {
        _referenceFileRequest = referenceFileRequest;
    }

    async Task<IEnumerable<DataSourceItem>> IAsyncDataSourceItemHandler.GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_referenceFileRequest.ProjectUId))
        {
            throw new PluginMisconfigurationException("Please fill in project first");
        }

        var client = new PhraseTmsClient(Creds);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{_referenceFileRequest.ProjectUId}/references?filename={context.SearchString}", Method.Get, Creds);
        var referenceFiles = await client.Paginate<ReferenceFileInfoDto>(request);
        return referenceFiles
            .Where(x => context.SearchString == null ||
                        x.Filename.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .Select(x => new DataSourceItem(x.UId, x.Filename));
    }
}