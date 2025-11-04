//using Apps.PhraseTMS.Actions;
//using Apps.PhraseTMS.Models.Jobs.Requests;
//using Apps.PhraseTMS.Models.Projects.Requests;
//using Apps.PhraseTMS.Webhooks;
//using Apps.PhraseTMS.Webhooks.Models.Requests;
//using Blackbird.Applications.Sdk.Common.Webhooks;
//using Newtonsoft.Json;
//using PhraseTMSTests.Base;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Tests.PhraseTMS;

//[TestClass]
//public class WebhookTests : TestBase
//{
//    private WebhookRequest CreateWebhookRequest(string fileName)
//    {
//        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
//        var projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
//        var body = File.ReadAllText($"{projectDirectory}/Webhooks/{fileName}");
//        return new WebhookRequest { Body = body };
//    }

//    private const string PROJECT_ID = "hGStrg0MLYmQtG0f66mj6f";
//    private const string JOB_ID = "AtSGZSMriZbu8F4L9Li7U1";
//    private const string SOURCE_FILE_ID = "KeSN9w8gTHrj8JFWMjV3F1";
//    private const string LANG = "de";
//    private const string WORKFLOW_STEP_ID = "7445";

//    [TestMethod]
//    public async Task Job_status_changed_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"),
//            new JobStatusChangedRequest { },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { },
//            new WorkflowStepOptionalRequest { },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { },
//            new OptionalSearchJobsQuery { },
//            null,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid != null);
//    }

//    [TestMethod]
//    public async Task Job_status_with_job_id_changed_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"), 
//            new JobStatusChangedRequest { },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { JobUId = JOB_ID },
//            new WorkflowStepOptionalRequest { },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { },
//            new OptionalSearchJobsQuery { },
//            null,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid != null);
//    }

//    [TestMethod]
//    public async Task Job_status_with_job_id_and_status_changed_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"),
//            new JobStatusChangedRequest { Status = new List<string> { "ACCEPTED" } },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { JobUId = JOB_ID },
//            new WorkflowStepOptionalRequest { },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { },
//            new OptionalSearchJobsQuery { },
//            null,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid != null);
//    }

//    [TestMethod]
//    public async Task Job_status_with_job_id_and_wrong_status_changed_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"),
//            new JobStatusChangedRequest { Status = new List<string> { "NEW" } },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { JobUId = JOB_ID },
//            new WorkflowStepOptionalRequest { },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { },
//            new OptionalSearchJobsQuery { },
//            null,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid == null);
//    }

//    [TestMethod]
//    public async Task Job_status_with_wrong_source_file_id_changed_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"),
//            new JobStatusChangedRequest { },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { },
//            new WorkflowStepOptionalRequest { },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { SourceFileId = "WRONG" },
//            new OptionalSearchJobsQuery { },
//            null,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid == null);
//    }

//    [TestMethod]
//    public async Task Job_status_with_right_workflow_step_file_id_changed_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"),
//            new JobStatusChangedRequest { },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { },
//            new WorkflowStepOptionalRequest { WorkflowStepId = WORKFLOW_STEP_ID },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { SourceFileId = SOURCE_FILE_ID },
//            new OptionalSearchJobsQuery { },
//            null,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid != null);
//    }

//    [TestMethod]
//    public async Task Job_status_with_right_workflow_step_wrong_language_file_id_changed_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"),
//            new JobStatusChangedRequest { },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { },
//            new WorkflowStepOptionalRequest { WorkflowStepId = WORKFLOW_STEP_ID },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { SourceFileId = SOURCE_FILE_ID },
//            new OptionalSearchJobsQuery { TargetLang = "nl"  },
//            null,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid == null);
//    }

//    [TestMethod]
//    public async Task Job_status_with_right_workflow_step_right_language_file_id_changed_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"),
//            new JobStatusChangedRequest { },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { },
//            new WorkflowStepOptionalRequest { WorkflowStepId = WORKFLOW_STEP_ID },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { SourceFileId = SOURCE_FILE_ID },
//            new OptionalSearchJobsQuery { TargetLang = LANG },
//            null,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid != null);
//    }

//    [TestMethod]
//    public async Task Job_status_with_last_workflow_step_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var result = await events.JobStatusChanged(
//            CreateWebhookRequest("job_status_changed.json"),
//            new JobStatusChangedRequest { },
//            new ProjectOptionalRequest { ProjectUId = PROJECT_ID },
//            new OptionalJobRequest { },
//            new WorkflowStepOptionalRequest { },
//            new MultipleWorkflowStepsOptionalRequest { },
//            new OptionalSourceFileIdRequest { SourceFileId = SOURCE_FILE_ID },
//            new OptionalSearchJobsQuery { },
//            true,
//            null,
//            null,
//            new MultipleSubdomains { }
//            );

//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result?.Result?.Uid != null);
//    }

//    [TestMethod]
//    public async Task Job_created_works()
//    {
//        var events = new WebhookList(InvocationContext);
//        var result = await events.JobCreation(CreateWebhookRequest("job_created.json"), new JobCreatedFilters { });
//        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
//        Assert.IsTrue(result.Result.Jobs.Any());
//    }

//    [TestMethod]
//    public async Task All_jobs_reached_status_async_works()
//    {
//        var events = new WebhookList(InvocationContext);

//        var projectUid = "mx1qPbEEGiN0vAKpIbFwa9";
//        var workflowStepId = "7447";
//        var jobStatuses = new List<string> { "COMPLETED_BY_LINGUIST", "NEW" };

//        var result = await events.HandleAllJobsReachedStatusAsync(
//            CreateWebhookRequest("job_status_changed_all_jobs_in_workflow_step.json"),
//            new WorkflowStepStatusRequest
//            {
//                ProjectUId = projectUid,
//                WorkflowStepId = workflowStepId,
//                JobStatuses = jobStatuses
//            },""
//        );

//        Assert.IsNotNull(result.Result);
//        Assert.IsTrue(result.Result.Jobs.Any());
//    }
//}
