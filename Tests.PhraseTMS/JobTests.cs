using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class JobTests : TestBase
    {
        [TestMethod]
        public async Task GetJob_IsSuccess()
        {
            var action = new JobActions(FileManager);
            var input1 = new ProjectRequest { ProjectUId= "JCrGdFaiOtGk0ykN02165h" };
            var input2 =new JobRequest { JobUId = "IuZXVLF91oTOuWdhKhfay3" };

            var result = await action.GetJob(InvocationContext.AuthenticationCredentialsProviders, input1, input2);

            Console.WriteLine(result.Uid);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateJob_IsSuccess()
        {
            var action = new JobActions(FileManager);
            var input1 = new ProjectRequest { ProjectUId = "JCrGdFaiOtGk0ykN02165h" };
            var input2 = new CreateJobRequest {  File = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "Crowdin.txt" } };

            var result = await action.CreateJob(InvocationContext.AuthenticationCredentialsProviders, input1, input2);

            Console.WriteLine(result.Uid);
            Assert.IsNotNull(result);
        }
    }
}
