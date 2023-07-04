using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.QualityAssurance.Responses;
using Apps.PhraseTMS.Models.QualityAssurance.Requests;
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
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/qualityAssurances/ignoredWarnings", 
                Method.Post, authenticationCredentialsProviders);
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
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles", Method.Get, authenticationCredentialsProviders);
            var response = client.Get<ResponseWrapper<List<LQAProfileDto>>>(request);
            return new ListLQAProfilesResponse()
            {
                Profiles = response.Content
            };
        }

        [Action("Delete LQA profile", Description = "Delete LQA profile")]
        public Task DeleteLQAProfile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteLQAProfileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles/{input.LQAProfileUId}", Method.Delete, authenticationCredentialsProviders);

            return client.ExecuteWithHandling(() => client.ExecuteAsync(request));
        }
    }
}
