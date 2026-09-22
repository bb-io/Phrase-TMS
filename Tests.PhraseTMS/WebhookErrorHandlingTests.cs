using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Webhooks;
using Apps.PhraseTMS.Webhooks.Models.Requests;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class WebhookErrorHandlingTests
{
    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public async Task Project_creation_acknowledges_invalid_json_without_firing()
    {
        var webhookList = CreateWebhookList(TestContext);

        var response = await webhookList.ProjectCreation(
            new WebhookRequest { Body = "{ invalid json" },
            new ProjectCreatedRequest(),
            new MultipleSubdomains(),
            new MultipleDomains());

        AssertAcknowledgedWithoutFiring(response);
    }

    [TestMethod]
    public async Task Job_status_changed_acknowledges_misconfiguration_exception_without_firing()
    {
        var webhookList = CreateWebhookList(TestContext);

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

        AssertAcknowledgedWithoutFiring(response);
    }

    private static void AssertAcknowledgedWithoutFiring<T>(WebhookResponse<T> response) where T : class
    {
        Assert.AreEqual(WebhookRequestType.Preflight, response.ReceivedWebhookRequestType);
        Assert.IsNull(response.Result);
        Assert.IsNull(response.HttpResponseMessage);
    }

    private static WebhookList CreateWebhookList(TestContext testContext)
    {
        var context = new InvocationContext
        {
            AuthenticationCredentialsProviders =
            [
                new AuthenticationCredentialsProvider("url", "https://example.com")
            ],
            Logger = TestBase.CreateTestLogger(() => testContext)
        };

        return new WebhookList(context);
    }
}
