using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Polling.Memory;
using Apps.PhraseTMS.Polling.Models.Requests;
using Apps.PhraseTMS.Polling.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.PhraseTMS.Polling;

[PollingEventList]
public class JobPollingList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    [PollingEvent("On all jobs in workflow step reached status", "Triggered when all jobs in a specific workflow step reach a status")]
    public async Task<PollingEventResponse<PollingMemory, JobsResponse>> OnAllJobsInWorkflowStepReachedStatus(
        PollingEventRequest<PollingMemory> request,
        [PollingEventParameter] OnAllJobsInWorkflowStepReachedStatusRequest onAllJobsInWorkflowRequest)
    {
        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = false
                }
            };
        }
        
        var client = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);
        var endpoint = $"/api2/v2/projects/{onAllJobsInWorkflowRequest.ProjectUId}/jobs";
        var apiRequest = new PhraseTmsRequest(endpoint, Method.Get,
            InvocationContext.AuthenticationCredentialsProviders);

        var response = await client.Paginate<JobDto>(apiRequest);
        response.ForEach(x => x.Project = new()
        {
            UId = onAllJobsInWorkflowRequest.ProjectUId
        });
        
        var relevantJobs = response
            .Where(x => x.WorkflowStep?.Id == onAllJobsInWorkflowRequest.WorkflowStepId)
            .ToList();

        if (!relevantJobs.Any())
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = false
                }
            };
        }

        var allJobsReachedRequiredWorkflowStepAndStatus = relevantJobs.All(x => x.Status == onAllJobsInWorkflowRequest.JobStatus);
        var triggered = allJobsReachedRequiredWorkflowStepAndStatus && !request.Memory.Triggered;
        return new()
        {
            FlyBird = triggered,
            Result = new()
            {
                Jobs = relevantJobs.Select(x => new JobResponse
                {
                    Uid = x.Uid,
                    ProjectUid = x.Project.UId,
                    Filename = x.Filename,
                    SourceLanguage = x.SourceLang,
                    Status = x.Status,
                    TargetLanguage = x.TargetLang,
                    WordCount = x.WordsCount
                }).ToList()
            },
            Memory = new()
            {
                LastPollingTime = DateTime.UtcNow,
                Triggered = triggered
            }
        };
    }
}