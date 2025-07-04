using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS
{
    [TestClass]
    public class JobActionsTests : TestBase
    {
        public const string PROJECT_ID = "hGStrg0MLYmQtG0f66mj6f";
        public const string JOB_ID = "AtSGZSMriZbu8F4L9Li7U1";

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
            var projectRequest = new ProjectRequest { ProjectUId = "RjmNQDYiERXq75syXIdHS2" };
            var searchQuery = new ListAllJobsQuery
            {
            };
            var jobStatuses = new JobStatusesRequest
            {
                
            };
            var workflowStep = new WorkflowStepOptionalRequest
            {
                WorkflowStepId = "7445",
                
            };

            bool? lqaScore = null;

            var result = await action.ListAllJobs(projectRequest, searchQuery, jobStatuses, workflowStep, lqaScore, null);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Jobs.Any());

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task Find_job_works()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };

            var result = await action.FindJob(projectRequest, "xQ2nG9N9BfHQ9ZerhQkjv3", new WorkflowStepRequest { WorkflowStepId = "7447" }, new TargetLanguageRequest { TargetLang = "de" });

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Uid);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        [TestMethod]
        public async Task Create_jobs_works()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var input2 = new CreateJobsRequest { useProjectFileImportSettings=true,  File = new FileReference { Name = "Business Trial Follow Up Email AIFW - 2567.html" } };

            var result = await action.CreateJobs(projectRequest, input2);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Jobs.Any());
            Assert.IsNotNull(result.SourceFileUid);
            Assert.IsTrue(result.Jobs.Select(x => x.SourceFileUid).All(x => x == result.SourceFileUid));
        }

        [TestMethod]
        public async Task Create_job_and_delete_jobworks()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = PROJECT_ID };
            var input2 = new CreateJobsRequest { 
                File = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "test_2.txt" } ,
                TargetLanguages = new List<string> { "nl" },
            };

            var result = await action.CreateJobs(projectRequest, input2);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);

            foreach(var item in result.Jobs)
            {
                await action.DeleteJob(projectRequest, new DeleteJobRequest { JobsUIds = new[] { item.Uid } });
            }
            
        }

        [TestMethod]
        public async Task UpdateSource_ValidData_Success()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectUId = "FwDrhxXNmSU15GNJiDVvQ7" };
            var jobs = new List<string>();
            jobs.Add("nc6TRX9h3upSeZV0177xF5");

            var input2 = new UpdateSourceRequest { 
                File = new Blackbird.Applications.Sdk.Common.Files.FileReference { Name = "test.txt" }, 
                Jobs =jobs };

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




        [TestMethod]
        public async Task ExportJobsToOnlineRepository_Success()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var input1 = new ProjectRequest { ProjectUId = "FwDrhxXNmSU15GNJiDVvQ7" };
            var jobs = new ExportJobsToOnlineRepositoryRequest { JobIds = ["wH062Uw4XpQ9qq8LZ53cf0", "nc6TRX9h3upSeZV0177xF5"] };

            var result = await action.ExportJobsToOnlineRepository(input1, jobs);
            foreach (var job in result.Jobs)
            {
                Console.WriteLine($"{job.Uid}");
            }
            
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Remove_Provers_works()
        {
            var action = new JobActions(InvocationContext, FileManager);
            var projectRequest = new ProjectRequest { ProjectUId = "RjmNQDYiERXq75syXIdHS2" };
            var jobRequest = new JobRequest { JobUId = "fk6DHhe6TcFk2Zu9VWTx21" };

            var result = await action.RemoveProvider(projectRequest, jobRequest);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Uid);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }




        [TestMethod]
        public async Task TestWebhook()
        {
            var jsonPayload1 = @"
            {
              ""JobParts"": [
                {
                  ""Uid"": ""4rUxlLJVAyVDdFgxGo9mf3"",
                  ""Project"": {
                    ""Uid"": ""GWzjor9lk8oR02NMa0fmDe"",
                    ""LastWorkflowLevel"": 2,
                    ""Name"": null,
                    ""subDomain"": null
                  },
                  ""Status"": ""COMPLETED_BY_LINGUIST"",
                  ""FileName"": ""PW  Test - Quote-009c1acc-3b43-40a4-934a-44128626f67d-jensenh@squareup.com-resubmission.json"",
                  ""TargetLang"": ""es_es"",
                  ""workflowLevel"": 1,
                  ""assignedTo"": [
                    {
                      ""Uid"": null
                    }
                  ]
                }
              ],
              ""metadata"": {
                ""project"": {
                  ""Uid"": ""GWzjor9lk8oR02NMa0fmDe"",
                  ""LastWorkflowLevel"": 0,
                  ""Name"": ""Eng Sandbox_test_ignore_jensen_2 - ICR - LQA"",
                  ""subDomain"": null
                }
              }
            }";

            var jsonPayload2 = @"
            {
              ""JobParts"": [
                {
                  ""Uid"": ""VeKgQDyPzzqC69U7r2Uil2"",
                  ""Project"": {
                    ""Uid"": ""GWzjor9lk8oR02NMa0fmDe"",
                    ""LastWorkflowLevel"": 2,
                    ""Name"": null,
                    ""subDomain"": null
                  },
                  ""Status"": ""COMPLETED_BY_LINGUIST"",
                  ""FileName"": ""PW  Test - Quote-009c1acc-3b43-40a4-934a-44128626f67d-jensenh@squareup.com-resubmission.json"",
                  ""TargetLang"": ""es_es"",
                  ""workflowLevel"": 2,
                  ""assignedTo"": []
                }
              ],
              ""metadata"": {
                ""project"": {
                  ""Uid"": ""GWzjor9lk8oR02NMa0fmDe"",
                  ""LastWorkflowLevel"": 0,
                  ""Name"": ""Eng Sandbox_test_ignore_jensen_2 - ICR - LQA"",
                  ""subDomain"": null
                }
              }
            }";

            var webhookRequest = new WebhookRequest
            {
                Body = JObject.Parse(jsonPayload2),
                Headers = new Dictionary<string, string>(),
                HttpMethod = HttpMethod.Post,
                Url = "https://example.com/webhooks/job-status-changed",
                QueryParameters = new Dictionary<string, string>()
            };

            var statusRequest = new JobStatusChangedRequest
            {
                Status = new[] { "COMPLETED_BY_LINGUIST" }
            };
            var projectOpt = new ProjectOptionalRequest {};
            var jobOpt = new OptionalJobRequest { };
            var workflowOpt = new WorkflowStepOptionalRequest { WorkflowStepId= "1081443" };
            var sourceFileOpt = new OptionalSourceFileIdRequest {};
            var jobsQuery = new OptionalSearchJobsQuery { };
            bool? lastLevel = null;
            string contain = "ICR - LQA";
            string notContain = null;
            var subdomains = new MultipleSubdomains {  };

            var actions = new WebhookList(InvocationContext);
            var response = await actions.JobStatusChanged(
                webhookRequest,
                statusRequest,
                projectOpt,
                jobOpt,
                workflowOpt,
                sourceFileOpt,
                jobsQuery,
                lastLevel,
                contain,
            notContain,
                subdomains
            );

            Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
            Assert.IsNotNull(response);
        }
    }
}
