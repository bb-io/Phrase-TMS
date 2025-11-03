using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class ProjectActionsTests : TestBase
    {
        public const string PROJECT_ID = "ayB1FFffK7hD0AXUAX9cPa";//"hGStrg0MLYmQtG0f66mj6f";

        [TestMethod]
        public async Task CreateProject_ValidData_Success()
        {
            var actions = new ProjectActions(InvocationContext, FileManager);

            var targetLangs = new List<string>();
            targetLangs.Add("en");

            var workflow = new List<string>();
            workflow.Add("lxHYoo7KWvWN6R7Ca99mz3");
            workflow.Add("NeLxbybjmGHy69Dq0QAsR0");

            var input = new CreateProjectRequest { SourceLanguage = "hu", TargetLanguages = targetLangs, Name = "this name", WorkflowSteps = workflow};

            var result = await actions.CreateProject(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.UId));
        }

        [TestMethod]
        public async Task CreateProjectFromTemplate_ValidData_Success()
        {
            var actions = new ProjectActions(InvocationContext, FileManager);

            var targetLangs = new List<string>();
            targetLangs.Add("en");

            var workflow = new List<string>();
            workflow.Add("lxHYoo7KWvWN6R7Ca99mz3");
            workflow.Add("NeLxbybjmGHy69Dq0QAsR0");

            var input = new CreateFromTemplateRequest {SourceLanguage = "hu", 
                TargetLanguages=targetLangs,
                TemplateUId= "lG56tOurwL9u21kRlsXgy3", 
                Name="template project with date with neccesary timezone", 
            };

            var result = await actions.CreateProjectFromTemplate(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.UId));
        }

        [TestMethod]
        public async Task Search_projects_works()
        {
            var actions = new ProjectActions(InvocationContext, FileManager);
            var result = await actions.ListAllProjects(new ListAllProjectsQuery { CreatedInLastHours = 1});

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Projects.Any());
        }

        [TestMethod]
        public async Task SearchTranslationMemories_works()
        {
            var actions = new TranslationMemoryActions(InvocationContext, FileManager);
            var result = await actions.SearchTranslationMemories(new Apps.PhraseTMS.Models.TranslationMemories.Requests.SearchTranslationMemoryRequest { });

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Get_project_works()
        {
            var actions = new ProjectActions(InvocationContext, FileManager);
            var result = await actions.GetProject(new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" });

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Name != null);
        }


        [TestMethod]
        public async Task EditProjectTranslationMemories_works()
        {
            var actions = new TranslationMemoryActions(InvocationContext, FileManager);

            //var reqLangReadWrite = new Apps.PhraseTMS.Models.TranslationMemories.Requests.EditProjectTransMemoriesRequest
            //{
            //    TransMemoryUids = new[] { "w1pV1izYniDtTQjV4iPD1s" }
            //};

            var reqLangReadWrite = new Apps.PhraseTMS.Models.TranslationMemories.Requests.EditProjectTransMemoriesRequest
            {
                TargetLanguage = "de",
                TransMemoryUids = new[] { "w1pV1izYniDtTQjV4iPD1s" },
                ReadModes = new[] { true },
                WriteModes = new[] { true },
                Penalties = new[] { 2 },
                Orders = new[] { 1 },
            };

            var input = reqLangReadWrite;

            var result = await actions.EditProjectTranslationMemories(
                new ProjectRequest { ProjectUId = "0SBo723Ge0wHfk0A1k1XWn0" },
                input
            );

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
        }
    }
}
