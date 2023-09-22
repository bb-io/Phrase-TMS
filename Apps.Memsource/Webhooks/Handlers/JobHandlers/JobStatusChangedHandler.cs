namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobStatusChangedHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "JOB_STATUS_CHANGED";

    public JobStatusChangedHandler() : base(SubscriptionEvent) { }
}