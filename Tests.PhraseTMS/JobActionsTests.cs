using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class JobActionsTests : TestBase
    {
        private JobActions _jobActions => new(InvocationContext, FileManager);

        public const string PROJECT_ID = "hGStrg0MLYmQtG0f66mj6f";
        public const string JOB_ID = "AtSGZSMriZbu8F4L9Li7U1";

        [TestMethod]
        public async Task GetJob_ValidIds_ShouldNotFailAndReturnNotEmptyResponse()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var jobRequest = new JobRequest { JobUId = JOB_ID };

            var result = await _jobActions.GetJob(projectRequest, jobRequest);

            PrintResponse(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
        }

        [TestMethod]
        public async Task Search_jobs_works()
        {
            var projectRequest = new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" };
            var searchQuery = new ListAllJobsQuery
            {
                NoteContains = "automated note",
            };
            var jobStatuses = new JobStatusesRequest
            {
                
            };
            var workflowStep = new WorkflowStepOptionalRequest
            {
                //WorkflowStepId = "7445",
                
            };
            bool? lqaScore = null;
            bool? lastWorkflowStep = null;

            var result = await _jobActions.ListAllJobs(
                projectRequest,
                searchQuery,
                jobStatuses,
                workflowStep,
                lqaScore,
                lastWorkflowStep);

            PrintResponse(result);
            Assert.IsTrue(result.Jobs.Any());
        }

        [TestMethod]
        public async Task Find_job_works()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var sourceFileUid = "xQ2nG9N9BfHQ9ZerhQkjv3";
            var workflowStep = new WorkflowStepRequest { WorkflowStepId = "7447" };
            var targetLanguage = new TargetLanguageRequest { TargetLang = "de" };

            var result = await _jobActions.FindJob(
                projectRequest,
                sourceFileUid,
                workflowStep,
                targetLanguage);

            PrintResponse(result);
        }

        [TestMethod]
        public async Task Create_jobs_works()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var createJobsRequest = new CreateJobsRequest
            {
                preTranslate = true,
                File = new FileReference { Name = "Semiconductor_Device_Control_Add_On_for_DUT_Validation_Course_Overview-en-zh_tw-Tr.mxliff" },
            };

            var result = await _jobActions.CreateJobs(projectRequest, createJobsRequest);

            PrintResponse(result);
            Assert.IsTrue(result.Jobs.Select(x => x.SourceFileUid).All(x => x == result.SourceFileUid));
        }

        [TestMethod]
        public async Task CreateJobs_FromInteroperableXliff_works()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var createJobsRequest = new CreateJobsRequest { File = new FileReference { Name = "basic-interoperable.xliff" } };

            var result = await _jobActions.CreateJobs(projectRequest, createJobsRequest);

            PrintResponse(result);
            Assert.IsTrue(result.Jobs.Select(x => x.SourceFileUid).All(x => x == result.SourceFileUid));
        }

        [TestMethod]
        public async Task Create_job_and_delete_jobworks()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var createJobsRequest = new CreateJobsRequest { 
                File = new FileReference { Name = "test_2.txt" } ,
                TargetLanguages = ["nl"],
            };

            var createJobsResult = await _jobActions.CreateJobs(projectRequest, createJobsRequest);

            foreach(var item in createJobsResult.Jobs)
            {
                var deleteJobRequest = new DeleteJobRequest { JobsUIds = [item.Uid] };
                await _jobActions.DeleteJob(projectRequest, deleteJobRequest);
            }

            PrintResponse(createJobsResult);
        }

        [TestMethod]
        public async Task UpdateSource_ValidData_Success()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };

            var updateSourceRequest = new UpdateSourceRequest { 
                File = new FileReference { Name = "test.txt" }, 
                Jobs = [JOB_ID],
            };

            var result = await _jobActions.UpdateSource(projectRequest, updateSourceRequest);

            PrintResponse(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
        }

        [TestMethod]
        public async Task Download_job_original_file_works()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var jobRequest = new JobRequest { JobUId = JOB_ID };

            var result = await _jobActions.DownloadOriginalFile(projectRequest, jobRequest);

            PrintResponse(result);
        }

        [TestMethod]
        public async Task Download_job_target_file_works()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var jobRequest = new JobRequest { JobUId = JOB_ID };

            var result = await _jobActions.DownloadTargetFile(projectRequest, jobRequest);

            PrintResponse(result);
        }

        //[TestMethod]
        //public async Task GetJobAnalysis_ValidIds_ShouldNotFailAndReturnNotEmptyResponse()
        //{
        //    var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
        //    var jobRequest = new JobRequest { JobUId = JOB_ID };
        //    var analysisRequest = new GetAnalysisRequest { AnalysisUId = "1376196960" };
        //    var result = await _jobActions.GetJobAnalysis(jobRequest, analysisRequest);
        //    PrintResponse(result.TotalInternalFuzzy);
        //}



        [TestMethod]
        public async Task ExportJobsToOnlineRepository_Success()
        {
            var projectRequest = new ProjectRequest { ProjectUId = "FwDrhxXNmSU15GNJiDVvQ7" };
            var jobs = new ExportJobsToOnlineRepositoryRequest
            {
                JobIds = ["wH062Uw4XpQ9qq8LZ53cf0", "nc6TRX9h3upSeZV0177xF5"]
            };

            var result = await _jobActions.ExportJobsToOnlineRepository(projectRequest, jobs);

            PrintResponse(result.Jobs.Select(j => j.Uid));
        }

        [TestMethod]
        public async Task GetSegments_Success()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var job = new JobRequest { JobUId = JOB_ID };

            var result = await _jobActions.GetSegmentsCount(projectRequest, job);

            PrintResponse(result);
        }

        [TestMethod]
        public async Task Remove_Provers_works()
        {
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var jobRequest = new JobRequest { JobUId = JOB_ID };

            var result = await _jobActions.RemoveProvider(projectRequest, jobRequest);

            PrintResponse(result);
        }
    }
}
