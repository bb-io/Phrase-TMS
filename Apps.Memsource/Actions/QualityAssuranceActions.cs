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
        public void AddIgnoredWarning(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] AddIgnoredWarningRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/qualityAssurances/ignoredWarnings", 
                Method.Post, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
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
        public ListLQAProfilesResponse ListLQAProfiles(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles", Method.Get, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
            var response = client.Get<ResponseWrapper<List<LQAProfileDto>>>(request);
            return new ListLQAProfilesResponse()
            {
                Profiles = response.Content
            };
        }

        [Action("Delete LQA profile", Description = "Delete LQA profile")]
        public void DeleteLQAProfile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteLQAProfileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles{input.LQAProfileUId}", Method.Delete, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
            client.Execute(request);
        }
    }
}
