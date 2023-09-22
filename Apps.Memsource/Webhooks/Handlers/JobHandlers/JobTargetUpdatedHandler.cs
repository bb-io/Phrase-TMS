namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobTargetUpdatedHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "JOB_TARGET_UPDATED";

    public JobTargetUpdatedHandler() : base(SubscriptionEvent) { }
}