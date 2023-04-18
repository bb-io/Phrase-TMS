using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.TranslationMemories.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Models.QualityAssurance.Responses;
using Apps.PhraseTMS.Models.Clients.Requests;
using Apps.PhraseTMS.Models.QualityAssurance.Requests;
using Apps.PhraseTMS.Models.Quotes.Request;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class QualityAssuranceActions
    {
        [Action("Add ignored warning", Description = "Add ignored warning")]
        public void AddIgnoredWarning(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] AddIgnoredWarningRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/qualityAssurances/ignoredWarnings", 
                Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                segments = new[]
                {
                    new
                    {
                        uid = input.SegmentUId,
                        warnings = new[]{ new { id = input.WarningId } }
                    }
                }
            });
            client.Execute(request);
        }

        [Action("Get list LQA profiles", Description = "Get list LQA profiles")]
        public ListLQAProfilesResponse ListLQAProfiles(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<ResponseWrapper<List<LQAProfileDto>>>(request);
            return new ListLQAProfilesResponse()
            {
                Profiles = response.Content
            };
        }

        [Action("Delete LQA profile", Description = "Delete LQA profile")]
        public void DeleteLQAProfile(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] DeleteLQAProfileRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles{input.LQAProfileUId}", Method.Delete, authenticationCredentialsProvider.Value);
            client.Execute(request);
        }
    }
}
