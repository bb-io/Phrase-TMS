using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Projects.Requests;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class SegmentDataHandler : BaseInvocable, IAsyncDataSourceHandler
{

    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private JobRequest JobRequest { get; set; }

    private ProjectRequest ProjectRequest { get; set; }

    public SegmentDataHandler(InvocationContext invocationContext, 
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] ProjectRequest projectRequest) : base(invocationContext)
    {
        JobRequest = jobRequest;
        ProjectRequest = projectRequest;
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new PhraseTmsClient(Creds);

        var segmentsCountEndpoint = $"/api2/v1/projects/{ProjectRequest.ProjectUId}/jobs/segmentsCount";
        var segmentsCountRequest = new PhraseTmsRequest(segmentsCountEndpoint, Method.Post, Creds);
        segmentsCountRequest.AddJsonBody(new
        {
            jobs = new[]
            {
                new {
                    uid = JobRequest.JobUId
                }
            }
        });
        var segmentCounts = await client.ExecuteWithHandling<SegmentCountDto>(segmentsCountRequest);
        var endpoint = $"/api2/v1/projects/{ProjectRequest.ProjectUId}/jobs/{JobRequest.JobUId}/segments"
            .WithQuery(new GetSegmentsQuery() { 
                BeginIndex = 0, 
                EndIndex = segmentCounts.SegmentsCountsResults.Where(s => s.JobPartUid == JobRequest.JobUId).First().Counts.SegmentsCount 
            });
        var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
        var segments = await client.ExecuteWithHandling<GetSegmentsResponse>(request);
        return segments.Segments
            .Where(x => context.SearchString == null ||
                        x.Source.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase) ||
                        x.Target.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToDictionary(x => x.Id, x => 
            {
                //var target = string.IsNullOrEmpty(x.Target) ? x.Target : "empty";
                //return $"{x.Source} -> {target}"; 
                return x.Source;
            });
    }
}