using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Filters.Transformations;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class JobActionsTests : TestBaseMultipleConnections
{
    public const string PROJECT_ID = "hGStrg0MLYmQtG0f66mj6f";
    public const string JOB_ID = "AtSGZSMriZbu8F4L9Li7U1";

    [TestMethod, ContextDataSource]
    public async Task GetJob_ValidIds_ShouldNotFailAndReturnNotEmptyResponse(InvocationContext context)
    {
        // Arrange
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var jobRequest = new JobRequest { JobUId = JOB_ID };

        // Act
        var result = await actions.GetJob(projectRequest, jobRequest);

        // Assert
        PrintResult(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
    }

    [TestMethod, ContextDataSource]
    public async Task Search_jobs_works(InvocationContext context)
    {
        // Arrange
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };
        var searchQuery = new ListAllJobsQuery { NoteContains = "automated note" };
        var jobStatuses = new JobStatusesRequest { };
        var workflowStep = new WorkflowStepOptionalRequest { };
        bool? lqaScore = null;
        bool? lastWorkflowStep = null;

        // Act
        var result = await actions.ListAllJobs(
            projectRequest,
            searchQuery,
            jobStatuses,
            workflowStep,
            lqaScore,
            lastWorkflowStep
        );

        // Assert
        PrintResult(result);
        Assert.IsTrue(result.Jobs.Any());
    }

    [TestMethod, ContextDataSource]
    public async Task Find_job_works(InvocationContext context)
    {
        // Arrange
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var sourceFileUid = "xQ2nG9N9BfHQ9ZerhQkjv3";
        var workflowStep = new WorkflowStepRequest { WorkflowStepId = "7447" };
        var targetLanguage = new TargetLanguageRequest { TargetLang = "de" };

        // Act
        var result = await actions.FindJob(
            projectRequest,
            sourceFileUid,
            workflowStep,
            targetLanguage
        );

        // Assert
        PrintResult(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Create_jobs_works(InvocationContext context)
    {
        // Arrange
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var createJobsRequest = new CreateJobsRequest
        {
            preTranslate = true,
            File = new FileReference { Name = "Semiconductor_Device_Control_Add_On_for_DUT_Validation_Course_Overview-en-zh_tw-Tr.mxliff" },
        };

        // Act
        var result = await actions.CreateJobs(projectRequest, createJobsRequest);

        // Assert
        PrintResult(result);
        Assert.IsTrue(result.Jobs.Select(x => x.SourceFileUid).All(x => x == result.SourceFileUid));
    }

    [TestMethod, ContextDataSource]
    public async Task Create_job_with_xliff2_import_settings(InvocationContext context)
    {
        // Arrange
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = "ltRSUSNtQ3qt7JuW8x14b6" };
        var createJobsRequest = new CreateJobsRequest
        {
            preTranslate = true,
            File = new FileReference { Name = "The Loire Valley_en-US.html.xlf" },
            useProjectFileImportSettings = true,
        };

        // Act
        var result = await actions.CreateJobs(projectRequest, createJobsRequest);

        // Assert
        PrintResult(result);
        Assert.IsTrue(result.Jobs.Select(x => x.SourceFileUid).All(x => x == result.SourceFileUid));
    }

    [TestMethod, ContextDataSource]
    public async Task CreateJobs_FromInteroperableXliff_works(InvocationContext context)
    {
        // Arrange
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var createJobsRequest = new CreateJobsRequest { File = new FileReference { Name = "basic-interoperable.xliff" } };

        // Act
        var result = await actions.CreateJobs(projectRequest, createJobsRequest);

        // Assert
        PrintResult(result);
        Assert.IsTrue(result.Jobs.Select(x => x.SourceFileUid).All(x => x == result.SourceFileUid));
    }

    [TestMethod, ContextDataSource]
    public async Task Create_job_and_delete_jobworks(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var createJobsRequest = new CreateJobsRequest { 
            File = new FileReference { Name = "test_2.txt" } ,
            TargetLanguages = ["nl"],
        };

        var createJobsResult = await actions.CreateJobs(projectRequest, createJobsRequest);

        foreach(var item in createJobsResult.Jobs)
        {
            var deleteJobRequest = new DeleteJobRequest { JobsUIds = [item.Uid] };
            await actions.DeleteJob(projectRequest, deleteJobRequest);
        }

        PrintResult(createJobsResult);
    }

    [TestMethod, ContextDataSource]
    public async Task UpdateSource_ValidData_Success(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };

        var updateSourceRequest = new UpdateSourceRequest { 
            File = new FileReference { Name = "test.txt" }, 
            Jobs = [JOB_ID],
        };

        var result = await actions.UpdateSource(projectRequest, updateSourceRequest);

        PrintResult(result);
        Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
    }

    [TestMethod, ContextDataSource]
    public async Task Download_job_original_file_works(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var jobRequest = new JobRequest { JobUId = JOB_ID };

        var result = await actions.DownloadOriginalFile(projectRequest, jobRequest);

        PrintResult(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Download_job_target_file_works(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var jobRequest = new JobRequest { JobUId = JOB_ID };

        var result = await actions.DownloadTargetFile(projectRequest, jobRequest);

        PrintResult(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Download_job_target_file_with_blacklake_metadata_works(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = "ltRSUSNtQ3qt7JuW8x14b6" };
        var jobRequest = new JobRequest { JobUId = "9aPeFiTrFyVZdddz0Q5Ys1" };

        var result = await actions.DownloadTargetFile(projectRequest, jobRequest);
        Assert.IsTrue(result.File.Name.EndsWith(".xlf"));
        var contentString = FileManager.ReadOutputAsString(result.File);

        var transformation = Transformation.Parse(contentString, result.File.Name);

        Assert.IsTrue(transformation.GetUnits().Any(x => x.Provenance.Translation.Tool == "Phrase TMS"));
        Assert.IsTrue(transformation.GetUnits().Any(x => x.Provenance.Translation.Person == "Mathijs Sonnemans"));

        PrintResult(result);
    }

    [TestMethod, ContextDataSource]
    public async Task ExportJobsToOnlineRepository_Success(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = "FwDrhxXNmSU15GNJiDVvQ7" };
        var jobs = new ExportJobsToOnlineRepositoryRequest
        {
            JobIds = ["wH062Uw4XpQ9qq8LZ53cf0", "nc6TRX9h3upSeZV0177xF5"]
        };

        var result = await actions.ExportJobsToOnlineRepository(projectRequest, jobs);

        PrintResult(result.Jobs.Select(j => j.Uid));
    }

    [TestMethod, ContextDataSource]
    public async Task GetSegments_Success(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var job = new JobRequest { JobUId = JOB_ID };

        var result = await actions.GetSegmentsCount(projectRequest, job);

        PrintResult(result);
    }

    [TestMethod, ContextDataSource]
    public async Task Remove_Provers_works(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        var jobRequest = new JobRequest { JobUId = JOB_ID };

        var result = await actions.RemoveProvider(projectRequest, jobRequest);

        PrintResult(result);
    }

    [TestMethod, ContextDataSource]
    public async Task UpdateTargetFile_IsSuccess(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = "" };
        var jobRequest = new JobRequest { JobUId = "" };
        var update = new UpdateTargetFileInput
        {
            File = new FileReference { Name = "" }
        };

        await actions.UpdateTargetFile(projectRequest, jobRequest, update);
    }

    [TestMethod, ContextDataSource]
    public async Task SplitJob_IsSuccess(InvocationContext context)
    {
        var actions = new JobActions(context, FileManager);
        var projectRequest = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };
        var jobRequest = new JobRequest { JobUId = "K4V6MO5RDBoDaFSdfxLqz1" };
        var split = new SplitJobRequest
        {
            //SegmentOrdinals = ["1", "5"],
            //PartCount = "2",
            //PartSize = "30",
            //WordCount = "20",

        };

        await actions.SplitJob(projectRequest, jobRequest, split);
    }
}
