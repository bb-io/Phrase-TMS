using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;

public class ProjectDeletionHandler(InvocationContext invocationContext)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent)
{
    const string SubscriptionEvent = "PROJECT_DELETED";
}