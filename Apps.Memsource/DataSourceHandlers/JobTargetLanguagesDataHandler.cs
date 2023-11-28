﻿using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Languages.Response;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.DataSourceHandlers
{
    public class JobTargetLanguagesDataHandler : BaseInvocable, IAsyncDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

        private ProjectRequest CreateJobRequest { get; set; }

        public JobTargetLanguagesDataHandler(InvocationContext invocationContext, [ActionParameter] ProjectRequest createJobRequest) : base(invocationContext)
        {
            CreateJobRequest = createJobRequest;
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(CreateJobRequest.ProjectUId))
            {
                throw new ArgumentException("Please fill in project first");
            }
            var client = new PhraseTmsClient(Creds);
            var projectRequest = new PhraseTmsRequest($"/api2/v1/projects/{CreateJobRequest.ProjectUId}", Method.Get, Creds);
            var project = await client.ExecuteWithHandling<ProjectDto>(projectRequest);

            var languageRequest = new PhraseTmsRequest("/api2/v1/languages", Method.Get, Creds);
            var languages = await client.ExecuteWithHandling<LanguagesResponse>(languageRequest);

            return languages.Languages.Where(l => project.TargetLangs.Contains(l.Code))
                .Where(x => context.SearchString == null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .Take(20)
                .ToDictionary(x => x.Code, x => x.Name);
        }
    }
}
