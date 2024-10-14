using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobContinuousUpdatedHandler(InvocationContext invocationContext)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent)
{
    const string SubscriptionEvent = "CONTINUOUS_JOB_UPDATED";
}