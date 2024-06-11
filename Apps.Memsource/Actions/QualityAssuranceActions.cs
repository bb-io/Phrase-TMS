using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.QualityAssurance.Responses;
using Apps.PhraseTMS.Models.QualityAssurance.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class QualityAssuranceActions
{
    [Action("Add ignored warning", Description = "Add a new ignored warning to the job segment")]
    public Task AddIgnoredWarning(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest,
        [ActionParameter] AddIgnoredWarningRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest(
            $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}/qualityAssurances/ignoredWarnings",
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
    public async Task<ListLqaProfilesResponse> ListLqaProfiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListLqaProfilesQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/lqa/profiles";
        var request =
            new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<LqaProfileDto>(request);

        return new()
        {
            Profiles = response
        };
    }

    [Action("Delete LQA profile", Description = "Delete specific LQA profile")]
    public Task DeleteLqaProfile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] DeleteLqaProfileRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/lqa/profiles/{input.LqaProfileUId}", Method.Delete,
            authenticationCredentialsProviders);

        return client.ExecuteWithHandling(request);
    }
}