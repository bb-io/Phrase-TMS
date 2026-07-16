using Apps.PhraseTMS.Dtos.Workflow;
using Apps.PhraseTMS.Models.WorkflowStep.Request;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.Actions;

[ActionList("Workflows")]
public class WorkflowActions(InvocationContext context) : PhraseInvocable(context)
{
    [Action("Get workflow step", Description = "Get details of a specific workflow step")]
    public async Task<WorkflowStepDto> GetWorkflowStep([ActionParameter] WorkflowStepRequest workflowStepInput)
    {
        var request = new RestRequest($"api2/v1/workflowSteps/{workflowStepInput.WorkflowStepId}");
        var result = await Client.ExecuteWithHandling<WorkflowStepDto>(request);
        return result;
    }
}