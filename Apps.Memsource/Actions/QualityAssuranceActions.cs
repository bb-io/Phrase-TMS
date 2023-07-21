﻿using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Extension;
using Apps.PhraseTMS.Models.Responses;
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
        public Task AddIgnoredWarning(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] AddIgnoredWarningRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest(
                $"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/qualityAssurances/ignoredWarnings",
                Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                segments = new[]
                {
                    new
                    {
                        uid = input.SegmentUId,
                        warnings = new[] { new { id = input.WarningId } }
                    }
                }
            });
            return client.ExecuteWithHandling(request);
        }

        [Action("Get list LQA profiles", Description = "Get list LQA profiles")]
        public async Task<ListLQAProfilesResponse> ListLQAProfiles(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListLQAProfilesQuery query)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);

            var endpoint = "/api2/v1/lqa/profiles";
            var request =
                new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

            var response = await client.ExecuteWithHandling<ResponseWrapper<List<LQAProfileDto>>>(request);

            return new ListLQAProfilesResponse
            {
                Profiles = response.Content
            };
        }

        [Action("Delete LQA profile", Description = "Delete LQA profile")]
        public Task DeleteLQAProfile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteLQAProfileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles/{input.LQAProfileUId}", Method.Delete,
                authenticationCredentialsProviders);

            return client.ExecuteWithHandling(request);
        }
    }
}