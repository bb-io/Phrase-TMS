using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Files.Requests;
using Apps.PhraseTMS.Models.Files.Responses;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Analysis.Responses;
using Apps.PhraseTMS.Models.Async;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class AnalysisActions
    {
        [Action("List analyses", Description = "List analyses")]
        public ListAnalysesResponse ListAnalyses(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] ListAnalysesRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v3/projects/{input.ProjectUId}/jobs/{input.JobUId}/analyses", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<ResponseWrapper<List<AnalysisDto>>>(request);
            return new ListAnalysesResponse()
            {
                Analyses = response.Content
            };
        }

        [Action("Get analysis", Description = "Get analysis")]
        public AnalysisDto GetAnalysis(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetAnalysisRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v3/analyses/{input.AnalysisUId}", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<AnalysisDto>(request);
            return response;
        }

        [Action("Create analysis", Description = "Create analysis")]
        public AsyncRequest CreateAnalysis(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateAnalysisRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v2/analyses", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(
                new
                {
                    jobs = input.JobsUIds.Select(j => new
                    {
                        uid = j
                    }).ToArray()
                }
            );
            var asyncRequest = client.PerformMultipleAsyncRequest(request, authenticationCredentialsProvider);
            return asyncRequest.First();
        }
    }
}
