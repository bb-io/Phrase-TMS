using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Analysis.Responses;
using Apps.PhraseTMS.Models.Async;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class AnalysisActions
    {
        [Action("List analyses", Description = "List all job's analyses")]
        public async Task<ListAnalysesResponse> ListAnalyses(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListAnalysesPathRequest path,
            [ActionParameter] ListAnalysesQueryRequest query)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);

            var endpoint = $"/api2/v3/projects/{path.ProjectUId}/jobs/{path.JobUId}/analyses"
                .WithQuery(query);
            
            var request = new PhraseTmsRequest(endpoint,
                Method.Get, authenticationCredentialsProviders);
            var response = await client.Paginate<AnalysisDto>(request);

            return new ListAnalysesResponse
            {
                Analyses = response
            };
        }

        [Action("Get analysis", Description = "Get job's analysis")]
        public Task<AnalysisDto> GetAnalysis(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetAnalysisRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v3/analyses/{input.AnalysisUId}", Method.Get,
                authenticationCredentialsProviders);
            return client.ExecuteWithHandling<AnalysisDto>(request);
        }

        [Action("Create analysis", Description = "Create a new analysis")]
        public AsyncRequest CreateAnalysis(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateAnalysisInput input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v2/analyses", Method.Post, authenticationCredentialsProviders);
            request.WithJsonBody(new CreateAnalysisRequest(input),  JsonConfig.Settings);
            
            var asyncRequest = client.PerformMultipleAsyncRequest(request, authenticationCredentialsProviders);
            return asyncRequest.First();
        }
    }
}