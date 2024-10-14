using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;

public class ProjectCreationHandler(InvocationContext invocationContext)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent)
{
    const string SubscriptionEvent = "PROJECT_CREATED";
}