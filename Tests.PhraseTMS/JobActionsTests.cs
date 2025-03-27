using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Newtonsoft.Json;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class JobActionsTests : TestBase
    {
        public const string PROJECT_ID = "hGStrg0MLYmQtG0f66mj6f";
        public const string JOB_ID = "e9ciferOOGVn0ySv0qqav7";

        [TestMethod]
        public async Task GetJob_ValidIds_ShouldNotFailAndReturnNotEmptyResponse()
        {
            var actions = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId= PROJECT_ID };
            var jobRequest = new JobRequest { JobUId = JOB_ID };

            var result = await actions.GetJob(projectRequest, jobRequest);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
        }

        [TestMethod]
        public async Task Search_jobs_works()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var searchQuery = new ListAllJobsQuery
            {
                
            };
            var jobStatuses = new JobStatusesRequest
            {
                
            };

            bool? lqaScore = null;

            var result = await action.ListAllJobs(projectRequest, searchQuery, jobStatuses, lqaScore);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Jobs.Any());

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task Create_jobs_works()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var input2 = new CreateJobsRequest {  File = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "test.html" } };

            var result = await action.CreateJobs(projectRequest, input2);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Jobs.Any());
        }

        [TestMethod]
        public async Task Create_job_and_delete_jobworks()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = "01ywgyyGh5FtqAUNhQEwj8" };
            var input2 = new CreateJobRequest { 
                File = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "[US] Toast Go® 2_en.html" } ,
                TargetLanguage = "nl-NL",
            };

            var result = await action.CreateJob(projectRequest, input2);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Uid != null);

            await action.DeleteJob(projectRequest, new DeleteJobRequest { JobsUIds = new[] { result.Uid } });
        }

        [TestMethod]
        public async Task UpdateSource_ValidData_Success()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectUId = PROJECT_ID };
            var jobs = new List<string>();
            jobs.Add(JOB_ID);

            var input2 = new UpdateSourceRequest { File = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "test.txt" }, Jobs =jobs };

            var result = await action.UpdateSource(input1, input2);

            Console.WriteLine(result.Uid);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
        }

        [TestMethod]
        public async Task Download_job_original_file_works()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var result = await action.DownloadOriginalFile(projectRequest, new JobRequest { JobUId = JOB_ID});

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsTrue(result.File != null);
        }

        [TestMethod]
        public async Task Download_job_target_file_works()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var result = await action.DownloadTargetFile(projectRequest, new JobRequest { JobUId = JOB_ID });

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsTrue(result.File != null);
        }

        //[TestMethod]
        //public async Task GetJobAnalysis_ValidIds_ShouldNotFailAndReturnNotEmptyResponse()
        //{
        //    var actions = new AnalysisActions(InvocationContext, FileManager);
        //    var projectRequest = new ProjectRequest { ProjectUId = "r4zn9RSwyO8NbtS72vT1H7" };
        //    var jobRequest = new JobRequest { JobUId = "r8yqpDhaYs84a51UZG0ORb" };
        //    var analysisRequest = new GetAnalysisRequest { AnalysisUId = "1376196960" };

        //    var result = await actions.GetJobAnalysis(jobRequest, analysisRequest);

        //    Console.WriteLine(result.TotalInternalFuzzy);
        //    Assert.IsNotNull(result);
        //}

    }
}
