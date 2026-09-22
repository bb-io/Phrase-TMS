using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.QualityAssurance.Responses;
using Apps.PhraseTMS.Polling.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS.Polling;

[PollingEventList("Quality assurance")]
public class LqaPollingList(InvocationContext invocationContext) : PhraseInvocable(invocationContext)
{
    [PollingEvent("On LQA report created", "Triggered when a new LQA report is available in a specific project"),
     MultipleEvents]
    public async Task<PollingEventResponse<PollingMemory, List<LqaResponse>>> OnLqaReportsCreated(
        PollingEventRequest<PollingMemory> request,
        [PollingEventParameter] ProjectRequest projectRequest,
        [PollingEventParameter] WorkflowStepOptionalRequest wfStep)
    {
        if (request.Memory is null)
        {
            return NoReports();
        }

        var jobActions = new JobActions(InvocationContext, null!);
        var projectJobs = await jobActions.ListAllJobs(projectRequest, new(), new(), wfStep, null, null);

        var getBatchRequest = new RestRequest("/api2/v1/lqa/assessments", Method.Post)
            .WithJsonBody(new
            {
                jobParts = projectJobs.Jobs.Select(x => new { uid = x.Uid }).ToList()
            });

        var dto = await Client.ExecuteWithHandling<GetLqasDto>(getBatchRequest);
        var nowUtc = DateTime.UtcNow;

        var createdWithTimePeriod = dto.AssessmentDetails
            .Where(x => x.FinishedDate.HasValue
                        && x.FinishedDate.Value.ToUniversalTime() >= request.Memory.LastPollingTime
                        && x.FinishedDate.Value.ToUniversalTime() < nowUtc)
            .ToList();

        return createdWithTimePeriod.Count == 0
            ? NoReports()
            : new()
            {
                FlyBird = true,
                Result = createdWithTimePeriod,
                Memory = new() { LastPollingTime = nowUtc }
            };
    }

    private static PollingEventResponse<PollingMemory, List<LqaResponse>> NoReports()
        => new()
        {
            FlyBird = false,
            Result = null,
            Memory = new() { LastPollingTime = DateTime.UtcNow }
        };
}
