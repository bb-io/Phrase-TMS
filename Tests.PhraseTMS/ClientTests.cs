using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.CustomFields;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Newtonsoft.Json;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class AnalysisTests : TestBase
    {
        public const string PROJECT_ID = "INLIOpS573UU4BbFBzs9v0";
        public const string JOB_ID = "UkZvUPhvw1QItyADWZIPp3";
        public const string ANALYSIS_ID = "JyaYpAIr8pF65xLsLfOOZ1";

        [TestMethod]
        public async Task Create_analysis_works()
        {
            var actions = new AnalysisActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId= PROJECT_ID };

            var result = await actions.CreateAnalysis(projectRequest, new CreateAnalysisInput { JobsUIds = new List<string> { JOB_ID } }, new WorkflowStepOptionalRequest { }, new ListAllJobsQuery { });

            Assert.IsTrue(result.Analyses.Any() && result.Analyses.All(x => x.Uid != null));
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task Search_project_analyses_works()
        {
            var actions = new AnalysisActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };

            var result = await actions.ListProjectAnalyses(projectRequest, new ListAnalysesQueryRequest { });

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsTrue(result.Analyses.Any() && result.Analyses.All(x => x.Uid != null), "Project has no analyses");
        }

        [TestMethod]
        public async Task Get_analysis_works()
        {
            var actions = new AnalysisActions(InvocationContext, FileManager);

            var result = await actions.GetJobAnalysis(new GetAnalysisRequest { AnalysisUId = ANALYSIS_ID });
            Assert.IsTrue(result.Uid != null);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task Download_analysis_file_works()
        {
            var actions = new AnalysisActions(InvocationContext, FileManager);

            var result = await actions.DownloadAnalysis(new GetAnalysisRequest { AnalysisUId = ANALYSIS_ID }, "JSON", null);
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task SetDateCustomField_works()
        {
            var actions = new CustomFieldsActions(InvocationContext);
            var date = DateTime.UtcNow.AddDays(15);
            await actions.SetDateCustomField(new ProjectRequest { ProjectUId= "0SBo723Ge0wHfk0A1k1XWn0" },new DateCustomFieldRequest {  FieldUId= "gtCnCd6aZ0SkaGXu8wETa1" }, date);
            Assert.IsTrue(true);
        }
    }
}
