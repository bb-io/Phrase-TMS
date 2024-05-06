using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class AnalysisDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private readonly string _projectUId;

    public AnalysisDataHandler(InvocationContext invocationContext, [ActionParameter] DownloadAnalysisRequest jobRequest) : base(invocationContext)
    {
        _projectUId = jobRequest.ProjectId;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(_projectUId))
        {
            throw new ArgumentException("Please fill in Project ID first");
        }
            
        var client = new PhraseTmsClient(Creds);
        var endpoint = $"/api2/v3/projects/{_projectUId}/analyses";
        var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
        var analysis = await client.Paginate<AnalysisDto>(request);
            
        return analysis
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToDictionary(x => x.UId, x => x.Name);
    }
}