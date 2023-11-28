using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;
using System.Collections;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Dtos;
using System.Security.Cryptography;

namespace Apps.PhraseTMS.DataSourceHandlers
{
    public class SegmentDataHandler : BaseInvocable, IAsyncDataSourceHandler
    {

        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        private GetJobRequest JobRequest { get; set; }

        public SegmentDataHandler(InvocationContext invocationContext, [ActionParameter] GetJobRequest jobRequest) : base(invocationContext)
        {
            JobRequest = jobRequest;
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            var client = new PhraseTmsClient(Creds);

            var segmentsCountEndpoint = $"/api2/v1/projects/{JobRequest.ProjectUId}/jobs/segmentsCount";
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
            var endpoint = $"/api2/v1/projects/{JobRequest.ProjectUId}/jobs/{JobRequest.JobUId}/segments"
                .WithQuery(new GetSegmentsQuery() { 
                    beginIndex = 0, 
                    endIndex = segmentCounts.SegmentsCountsResults.Where(s => s.JobPartUid == JobRequest.JobUId).First().Counts.SegmentsCount 
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
                    var target = string.IsNullOrEmpty(x.Target) ? x.Target : "empty";
                    return $"{x.Source} -> {target}"; 
                });
        }
    }
}
