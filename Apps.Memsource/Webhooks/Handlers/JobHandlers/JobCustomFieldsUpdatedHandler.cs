using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobCustomFieldsUpdatedHandler(InvocationContext invocationContext)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent)
{
    private const string SubscriptionEvent = "JOB_CUSTOM_FIELDS_UPDATED";
}
