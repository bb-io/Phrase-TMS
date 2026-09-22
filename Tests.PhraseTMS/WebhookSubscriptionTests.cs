using Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;
using Blackbird.Applications.Sdk.Common.Invocation;
using PhraseTMSTests.Base;

namespace Tests.PhraseTMS;

[TestClass]
public class WebhookSubscriptionTests : TestBase
{
    private const string PayloadUrl = "https://webhook.site/8f6a6a4e-0e6a-4d6a-9a5e-blackbird-test";

    [TestMethod, ContextDataSource]
    public async Task Subscription_lifecycle_validates_while_subscribed(InvocationContext context)
    {
        var handler = new JobCreationHandler(context);
        var values = new Dictionary<string, string> { ["payloadUrl"] = PayloadUrl };

        await handler.SubscribeAsync(context.AuthenticationCredentialsProviders, values);

        try
        {
            var validation = await handler.ValidateSubscription(context.AuthenticationCredentialsProviders, values);

            Assert.IsTrue(validation.IsValid, validation.Message);
        }
        finally
        {
            await handler.UnsubscribeAsync(context.AuthenticationCredentialsProviders, values);
        }
    }

    [TestMethod, ContextDataSource]
    public async Task Validation_reports_missing_subscription(InvocationContext context)
    {
        var handler = new JobCreationHandler(context);
        var values = new Dictionary<string, string> { ["payloadUrl"] = $"{PayloadUrl}-never-subscribed" };

        var validation = await handler.ValidateSubscription(context.AuthenticationCredentialsProviders, values);

        Assert.IsFalse(validation.IsValid);
        Assert.IsFalse(string.IsNullOrWhiteSpace(validation.Message));
        Console.WriteLine(validation.Message);
    }
}
