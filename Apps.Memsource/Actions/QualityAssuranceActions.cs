using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.QualityAssurance.Responses;
using Apps.PhraseTMS.Models.QualityAssurance.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class QualityAssuranceActions
{
    [Action("Add ignored warning", Description = "Add a new ignored warning to the job segment")]
    public Task AddIgnoredWarning(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] AddIgnoredWarningRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest(
            $"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/qualityAssurances/ignoredWarnings",
            Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(new
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

    [Action("List LQA profiles", Description = "List all LQA profiles")]
    public async Task<ListLQAProfilesResponse> ListLQAProfiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListLQAProfilesQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/lqa/profiles";
        var request =
            new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<LQAProfileDto>(request);

        return new ListLQAProfilesResponse
        {
            Profiles = response
        };
    }

    [Action("Delete LQA profile", Description = "Delete specific LQA profile")]
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