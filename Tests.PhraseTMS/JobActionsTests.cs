using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class JobActionsTests : TestBase
    {
        [TestMethod]
        public async Task GetJob_ValidIds_ShouldNotFailAndReturnNotEmptyResponse()
        {
            var actions = new JobActions(FileManager);
            var projectRequest = new ProjectRequest { ProjectUId= "JCrGdFaiOtGk0ykN02165h" };
            var jobRequest = new JobRequest { JobUId = "IuZXVLF91oTOuWdhKhfay3" };

            var result = await actions.GetJob(InvocationContext.AuthenticationCredentialsProviders, projectRequest, jobRequest);

            Console.WriteLine(result.Uid);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
        }
        
        [TestMethod]
        public async Task ListAllJobs_ValidIdsWithCompletedStatus_ShouldNotFail()
        {
            var action = new JobActions(FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = "Z6WgM4mN4crBbpYTB2T3M2" };
            var searchQuery = new ListAllJobsQuery
            {
                WorkflowLevel = 3
            };
            var jobStatuses = new JobStatusesRequest
            {
                Statuses = new[] { "COMPLETED" },
            };
            
            bool? lqaScore = null;

            var result = await action.ListAllJobs(InvocationContext.AuthenticationCredentialsProviders, projectRequest, searchQuery, jobStatuses, lqaScore);
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Jobs.Any());

            foreach (var job in result.Jobs)
            {
                Console.WriteLine($"[{job.Uid}] ({job.Status}) - {job.Filename}");
            }
        }

        [TestMethod]
        public async Task CreateJob_ValidData_ShouldNotFailAndReturnNotEmptyResponse()
        {
            var action = new JobActions(FileManager);
            var input1 = new ProjectRequest { ProjectUId = "JCrGdFaiOtGk0ykN02165h" };
            var input2 = new CreateJobRequest {  File = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "Crowdin.txt" } };

            var result = await action.CreateJob(InvocationContext.AuthenticationCredentialsProviders, input1, input2);

            Console.WriteLine(result.Uid);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
        }

        [TestMethod]
        public async Task UpdateSource_ValidData_Success()
        {
            var action = new JobActions(FileManager);
            var input1 = new ProjectRequest { ProjectUId = "JCrGdFaiOtGk0ykN02165h" };
            var input2 = new UpdateSourceRequest { File = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "Crowdin.txt" } };

            var result = await action.UpdateSource(InvocationContext.AuthenticationCredentialsProviders, input1, input2);

            Console.WriteLine(result.Uid);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Uid));
        }
    }
}
