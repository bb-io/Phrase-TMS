using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class ProjectActionsTests : TestBase
    {

        [TestMethod]
        public async Task CreateProject_ValidData_Success()
        {
            var action = new ProjectActions(FileManager);

            var targetLangs = new List<string>();
            targetLangs.Add("en");

            var workflow = new List<string>();
            workflow.Add("lxHYoo7KWvWN6R7Ca99mz3");
            workflow.Add("NeLxbybjmGHy69Dq0QAsR0");

            var input = new CreateProjectRequest { SourceLanguage = "hu", TargetLanguages = targetLangs, Name = "this name", WorkflowSteps = workflow };

            var result = await action.CreateProject(InvocationContext.AuthenticationCredentialsProviders, input);

            Console.WriteLine(result.UId);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.UId));
        }

        [TestMethod]
        public async Task CreateProjectFromTemplate_ValidData_Success()
        {
            var action = new ProjectActions(FileManager);

            var targetLangs = new List<string>();
            targetLangs.Add("en");

            var workflow = new List<string>();
            workflow.Add("lxHYoo7KWvWN6R7Ca99mz3");
            workflow.Add("NeLxbybjmGHy69Dq0QAsR0");

            var input = new CreateFromTemplateRequest {SourceLanguage = "hu", TargetLanguages=targetLangs,TemplateUId= "lG56tOurwL9u21kRlsXgy3", Name="template project", WorkflowSteps = workflow };

            var result = await action.CreateProjectFromTemplate(InvocationContext.AuthenticationCredentialsProviders, input);

            Console.WriteLine(result.UId);
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.UId));
        }
    }
}
