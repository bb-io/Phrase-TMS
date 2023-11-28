using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Languages.Response;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using System.Collections;

namespace Apps.PhraseTMS.DataSourceHandlers
{
    public class JobDataHandler : BaseInvocable, IAsyncDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

        private ProjectRequest ProjectRequest { get; set; }

        public JobDataHandler(InvocationContext invocationContext, [ActionParameter] ProjectRequest projectRequest) : base(invocationContext)
        {
            ProjectRequest = projectRequest;
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(ProjectRequest.ProjectUId))
            {
                throw new ArgumentException("Please fill in project first");
            }
            var client = new PhraseTmsClient(Creds);
            var request = new PhraseTmsRequest($"/api2/v2/projects/{ProjectRequest.ProjectUId}/jobs", Method.Get, Creds);
            var jobs = await client.Paginate<JobDto>(request);
            return jobs
                .Where(x => context.SearchString == null ||
                            x.Filename.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(20)
                .ToDictionary(x => x.Uid, x => $"{x.Filename} ({x.InnerId})");
        }
    }
}
