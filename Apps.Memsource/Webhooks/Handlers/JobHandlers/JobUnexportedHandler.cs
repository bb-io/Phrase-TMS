using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobUnexportedHandler(InvocationContext invocationContext)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent)
{
    const string SubscriptionEvent = "JOB_UNEXPORTED";
}