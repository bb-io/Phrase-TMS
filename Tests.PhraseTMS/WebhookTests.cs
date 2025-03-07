using Apps.PhraseTMS.Actions;
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
            new ProjectOptionalRequest { ProjectUId = "hGStrg0MLYmQtG0f66mj6f" },
            new OptionalJobRequest { JobUId = "WRZha0uJNtgssOTzlujHh7" },
            null,
            null,
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.IsTrue(result.Result.Uid != null);
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
