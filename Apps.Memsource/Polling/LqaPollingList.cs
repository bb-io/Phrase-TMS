using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.QualityAssurance.Responses;
using Apps.PhraseTMS.Polling.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS.Polling;

[PollingEventList]
public class LqaPollingList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    [PollingEvent("On LQA reports created", "Triggered when new reports can be downloaded within a specific project")]
    public async Task<PollingEventResponse<PollingMemory, SearchLqaResponse>> OnLqaReportsCreated(
        PollingEventRequest<PollingMemory> request,
        [PollingEventParameter] ProjectRequest projectRequest)
    {
        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastPollingTime = DateTime.UtcNow
                }
            };
        }

        var jobActions = new JobActions(null!);
        var projectJobs = await jobActions.ListAllJobs(InvocationContext.AuthenticationCredentialsProviders,
            projectRequest, new());

        var client = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);
        var getBatchRequest = new PhraseTmsRequest("/api2/v1/lqa/assessments", Method.Post,
                InvocationContext.AuthenticationCredentialsProviders)
            .WithJsonBody(new
            {
                jobParts = projectJobs.Jobs.Select(x => new { uid = x.Uid }).ToList()
            });
        
        var dto = await client.ExecuteWithHandling<GetLqasDto>(getBatchRequest);
        var createdWithTimePeriod = dto.AssessmentDetails
            .Where(x => x.FinishedDate.HasValue 
                        && x.FinishedDate.Value.ToUniversalTime() >= request.Memory.LastPollingTime 
                        && x.FinishedDate.Value.ToUniversalTime() < DateTime.UtcNow)
            .ToList();
        
        return new()
        {
            FlyBird = createdWithTimePeriod.Count > 0,
            Result = new() { LanguageQualityAssessments = createdWithTimePeriod },
            Memory = new()
            {
                LastPollingTime = DateTime.UtcNow
            }
        };
    }
}