using PhraseTMSTests.Base;
using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Tests.PhraseTMS;

[TestClass]
public class AnalysisTests : TestBaseMultipleConnections
{
    public const string PROJECT_ID = "INLIOpS573UU4BbFBzs9v0";
    public const string JOB_ID = "UkZvUPhvw1QItyADWZIPp3";

    [TestMethod, ContextDataSource(ConnectionTypes.Credentials)]
    public async Task Create_analysis_works(InvocationContext context)
    {
        // Arrange
        var actions = new AnalysisActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var analysis = new CreateAnalysisInput { JobsUIds = new List<string> { JOB_ID } };
        var workflowStep = new WorkflowStepOptionalRequest { };
        var jobsQuery = new ListAllJobsQuery { };

        // Act
        var result = await actions.CreateAnalysis(projectRequest, analysis, workflowStep, jobsQuery);

        // Assert
        PrintResult(result);
        Assert.IsTrue(result.Analyses.Any() && result.Analyses.All(x => x.Uid != null));
    }

    [TestMethod, ContextDataSource(ConnectionTypes.ApiToken)]
    public async Task ExportProjectAnalysis_IsSuccess(InvocationContext context)
    {
        // Arrange
        var actions = new AnalysisActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = "kdr3NNw8RLX50ynOl9a9b4" };

        // Act
        var result = await actions.ExportProjectAnalysis(projectRequest);

        // Assert
        TestContext.WriteLine(result.ExportedAnalysis.Name);
    }
}
