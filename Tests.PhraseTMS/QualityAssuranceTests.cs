using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Conversations.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.QualityAssurance.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class QualityAssuranceTests : TestBaseMultipleConnections
{
    [TestMethod, ContextDataSource]
    public async Task Start_LQA_assessment_works(InvocationContext context)
    {
        // Arrange
        var actions = new QualityAssuranceActions(context, FileManager);
        var jobRequest = new JobRequest { JobUId = "1d9PrdgW6wjEsq1v18hlVu" };

        // Act
        var result = await actions.StartLqaAssessment(jobRequest);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Finish_LQA_assessment_works(InvocationContext context)
    {
        // Arrange
        var actions = new QualityAssuranceActions(context, FileManager);
        var jobRequest = new JobRequest { JobUId = "1d9PrdgW6wjEsq1v18hlVu" };

        await actions.StartLqaAssessment(jobRequest);

        var input = new FinishLqaAssessmentRequest
        {
            OverallFeedback = "Finished by automated test"
        };

        // Act
        var result = await actions.FinishLqaAssessment(jobRequest, input);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Create_LQA_conversation_works(InvocationContext context)
    {
        // Arrange
        var actions = new ConversationActions(context);
        var projectRequest = new ProjectRequest { ProjectUId = "ayB1FFffK7hD0AXUAX9cPa" };
        var jobRequest = new JobRequest { JobUId = "1d9PrdgW6wjEsq1v18hlVu" };
        var input = new CreateLqaConversationRequest
        {
            LqaDescription = "Created by automated test",
            SegmentId = "v0fJsO900s5SWOwj3_dc2:2",
            ErrorCategoryIds = ["2"],
            SeverityIds = ["2"],
            OriginValues = ["HUMAN"],
            TransGroupId = 2
        };

        // Act
        var result = await actions.CreateLqaConversation(projectRequest, jobRequest, input);

        // Assert
        PrintResult(result);
        Assert.IsNotNull(result);
    }
}
