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
    [TestMethod]
    public async Task Job_status_changed_works()
    {
        var events = new WebhookList(InvocationContext);

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;

        var body = File.ReadAllText($"{projectDirectory}/Webhooks/job_status_changed.json");

        var result = await events.JobStatusChanged(
            new WebhookRequest { Body = body }, 
            new JobStatusChangedRequest { },
            new ProjectOptionalRequest { ProjectUId = "INLIOpS573UU4BbFBzs9v0" },
            new OptionalJobRequest { JobUId = "yBQKRcaCYpHaedC2i3jr71" },
            null,
            null,
            null
            );

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }
}
