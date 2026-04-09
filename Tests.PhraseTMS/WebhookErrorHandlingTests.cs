using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Tests.PhraseTMS;

[TestClass]
public class WebhookErrorHandlingTests
{
    [TestMethod]
    public async Task Project_creation_returns_bad_request_for_invalid_json()
    {
        var webhookList = CreateWebhookList();

        var response = await webhookList.ProjectCreation(
            new WebhookRequest { Body = "{ invalid json" },
            new ProjectCreatedRequest(),
            new MultipleSubdomains(),
            new MultipleDomains());

        Assert.AreEqual(WebhookRequestType.Preflight, response.ReceivedWebhookRequestType);
        Assert.IsNotNull(response.HttpResponseMessage);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.HttpResponseMessage.StatusCode);

        var responseBody = await response.HttpResponseMessage.Content.ReadAsStringAsync();
        StringAssert.Contains(responseBody, "\"webhook\": \"PhraseTMSProjectCreation\"");
        StringAssert.Contains(responseBody, "\"message\":");
        StringAssert.Contains(responseBody, "\"stackTrace\":");
    }

    [TestMethod]
    public async Task Job_status_changed_returns_bad_request_for_misconfiguration_exception()
    {
        var webhookList = CreateWebhookList();

        var response = await webhookList.JobStatusChanged(
            new WebhookRequest { Body = "{}" },
            new JobStatusChangedRequest(),
            new ProjectOptionalRequest(),
            new OptionalJobRequest { JobUId = "job-id" },
            new WorkflowStepOptionalRequest(),
            new MultipleWorkflowStepsOptionalRequest(),
            new OptionalSourceFileIdRequest(),
            new OptionalSearchJobsQuery(),
            null,
            null,
            null,
            null,
            null,
            new MultipleSubdomains());

        Assert.AreEqual(WebhookRequestType.Preflight, response.ReceivedWebhookRequestType);
        Assert.IsNotNull(response.HttpResponseMessage);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.HttpResponseMessage.StatusCode);

        var responseBody = await response.HttpResponseMessage.Content.ReadAsStringAsync();
        StringAssert.Contains(responseBody, "If Job ID is specified in the inputs you must also specify the Project ID");
        StringAssert.Contains(responseBody, "\"webhook\": \"PhraseTMSJobStatusChanged\"");
    }

    private static WebhookList CreateWebhookList()
    {
        var context = new InvocationContext
        {
            AuthenticationCredentialsProviders =
            [
                new AuthenticationCredentialsProvider("url", "https://example.com")
            ]
        };

        return new WebhookList(context);
    }
}
