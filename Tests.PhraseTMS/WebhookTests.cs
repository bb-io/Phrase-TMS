using Apps.PhraseTMS.Actions;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using PhraseTMSTests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.PhraseTMS;

[TestClass]
public class WebhookTests : TestBase
{
    private WebhookRequest CreateWebhookRequest(string fileName)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
        var body = File.ReadAllText($"{projectDirectory}/Webhooks/{fileName}");
        return new WebhookRequest { Body = body };
    }

    [TestMethod]
    public async Task Job_status_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"),
            new JobStatusChangedRequest { },
            new OptionalJobRequest { },
            new WorkflowStepOptionalRequest { },
            new OptionalSourceFileIdRequest { },
            new OptionalSearchJobsQuery { },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid != null);
    }

    [TestMethod]
    public async Task Job_status_with_job_id_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"), 
            new JobStatusChangedRequest { },
            new OptionalJobRequest { JobUId = "f0SeyVX72ri5diSoDEWAg3" },
            new WorkflowStepOptionalRequest { },
            new OptionalSourceFileIdRequest { },
            new OptionalSearchJobsQuery { },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid != null);
    }

    [TestMethod]
    public async Task Job_status_with_job_id_and_status_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"),
            new JobStatusChangedRequest { Status = new List<string> { "ASSIGNED" } },
            new OptionalJobRequest { JobUId = "f0SeyVX72ri5diSoDEWAg3" },
            new WorkflowStepOptionalRequest { },
            new OptionalSourceFileIdRequest { },
            new OptionalSearchJobsQuery { },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid != null);
    }

    [TestMethod]
    public async Task Job_status_with_job_id_and_wrong_status_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"),
            new JobStatusChangedRequest { Status = new List<string> { "NEW" } },
            new OptionalJobRequest { JobUId = "f0SeyVX72ri5diSoDEWAg3" },
            new WorkflowStepOptionalRequest { },
            new OptionalSourceFileIdRequest { },
            new OptionalSearchJobsQuery { },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid == null);
    }

    [TestMethod]
    public async Task Job_status_with_source_file_id_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"),
            new JobStatusChangedRequest { },
            new OptionalJobRequest { },
            new WorkflowStepOptionalRequest { },
            new OptionalSourceFileIdRequest { SourceFileId = "LNQbXevW7l9b7dYj6kDxV3" },
            new OptionalSearchJobsQuery { },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid != null);
    }

    [TestMethod]
    public async Task Job_status_with_wrong_source_file_id_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"),
            new JobStatusChangedRequest { },
            new OptionalJobRequest { },
            new WorkflowStepOptionalRequest { },
            new OptionalSourceFileIdRequest { SourceFileId = "WRONG" },
            new OptionalSearchJobsQuery { },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid == null);
    }

    [TestMethod]
    public async Task Job_status_with_wrong_workflow_step_file_id_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"),
            new JobStatusChangedRequest { },
            new OptionalJobRequest { },
            new WorkflowStepOptionalRequest { WorkflowStepId = "7447" },
            new OptionalSourceFileIdRequest { SourceFileId = "LNQbXevW7l9b7dYj6kDxV3" },
            new OptionalSearchJobsQuery { },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid == null);
    }

    [TestMethod]
    public async Task Job_status_with_right_workflow_step_file_id_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"),
            new JobStatusChangedRequest { },
            new OptionalJobRequest { },
            new WorkflowStepOptionalRequest { WorkflowStepId = "7445" },
            new OptionalSourceFileIdRequest { SourceFileId = "LNQbXevW7l9b7dYj6kDxV3" },
            new OptionalSearchJobsQuery { },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid != null);
    }

    [TestMethod]
    public async Task Job_status_with_right_workflow_step_wrong_language_file_id_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var result = await events.JobStatusChanged(
            CreateWebhookRequest("job_status_changed.json"),
            new JobStatusChangedRequest { },
            new OptionalJobRequest { },
            new WorkflowStepOptionalRequest { WorkflowStepId = "7445" },
            new OptionalSourceFileIdRequest { SourceFileId = "LNQbXevW7l9b7dYj6kDxV3" },
            new OptionalSearchJobsQuery { TargetLang = "nl"  },
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result?.Result?.Uid == null);
    }

    [TestMethod]
    public async Task Job_created_works()
    {
        var events = new WebhookList(InvocationContext);
        var result = await events.JobCreation(CreateWebhookRequest("job_created.json"));
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result.Result.Jobs.Any());
    }
}
