using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.DataSourceHandlers
{
    public class AnalysisDataHandler : BaseInvocable, IAsyncDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

        private GetJobRequest JobRequest { get; set; }

        private ProjectRequest ProjectRequest { get; set; }

        public AnalysisDataHandler(InvocationContext invocationContext, 
            [ActionParameter] GetJobRequest jobRequest,
            [ActionParameter] ProjectRequest projectRequest) : base(invocationContext)
        {
            ProjectRequest = projectRequest;
            JobRequest = jobRequest;
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(JobRequest.JobUId))
            {
                throw new ArgumentException("Please fill in job first");
            }
            var client = new PhraseTmsClient(Creds);
            var endpoint = $"/api2/v3/projects/{ProjectRequest.ProjectUId}/jobs/{JobRequest.JobUId}/analyses";
            var request = new PhraseTmsRequest(endpoint, Method.Get, Creds);
            var analysis = await client.Paginate<AnalysisDto>(request);
            return analysis
                .Where(x => context.SearchString == null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(20)
                .ToDictionary(x => x.UId, x => x.Name);
        }
    }
}
