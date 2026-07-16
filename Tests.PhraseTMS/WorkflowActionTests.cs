using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Models.WorkflowStep.Request;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class WorkflowActionTests : TestBaseMultipleConnections
{
    [TestMethod, ContextDataSource(ConnectionTypes.ApiToken)]
    public async Task GetWorkflowStep_ReturnsWorkflowStep(InvocationContext context)
    {
        // Arrange
        var actions = new WorkflowActions(context);
        var workflowStepInput = new WorkflowStepRequest { WorkflowStepId = "1226626" };

        // Act
        var result = await actions.GetWorkflowStep(workflowStepInput);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }
}