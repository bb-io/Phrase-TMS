using PhraseTMSTests.Base;
using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Models.CustomFields;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Tests.PhraseTMS;

[TestClass]
public class AnalysisTests : TestBaseMultipleConnections
{
    public const string PROJECT_ID = "INLIOpS573UU4BbFBzs9v0";
    public const string JOB_ID = "UkZvUPhvw1QItyADWZIPp3";
    public const string ANALYSIS_ID = "JyaYpAIr8pF65xLsLfOOZ1";

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

    [TestMethod, ContextDataSource]
    public async Task Search_project_analyses_works(InvocationContext context)
    {
        // Arrange
        var actions = new AnalysisActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };

        // Act
        var result = await actions.ListProjectAnalyses(projectRequest, new ListAnalysesQueryRequest { });

        // Assert
        PrintResult(result);
        Assert.IsTrue(result.Analyses.Any() && result.Analyses.All(x => x.Uid != null), "Project has no analyses");
    }

    [TestMethod, ContextDataSource]
    public async Task Get_analysis_works(InvocationContext context)
    {
        // Arrange
        var actions = new AnalysisActions(context, FileManager);

        // Act
        var result = await actions.GetJobAnalysis(new GetAnalysisRequest { AnalysisUId = ANALYSIS_ID });

        // Assert
        PrintResult(result);
        Assert.IsTrue(result.Uid != null);
    }

    [TestMethod, ContextDataSource]
    public async Task Download_analysis_file_works(InvocationContext context)
    {
        // Arrange
        var actions = new AnalysisActions(context, FileManager);

        // Act
        var result = await actions.DownloadAnalysis(new GetAnalysisRequest { AnalysisUId = ANALYSIS_ID }, "JSON", null);

        // Assert
        PrintResult(result);
    }

    [TestMethod, ContextDataSource]
    public async Task SetDateCustomField_works(InvocationContext context)
    {
        // Arrange
        var actions = new CustomFieldsActions(context);
        var date = DateTime.UtcNow.AddDays(15);
        var project = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };
        var dateCustomField = new DateCustomFieldRequest { FieldUId = "gtCnCd6aZ0SkaGXu8wETa1" };

        // Act
        await actions.SetDateCustomField(project, dateCustomField, date);
    }
}
