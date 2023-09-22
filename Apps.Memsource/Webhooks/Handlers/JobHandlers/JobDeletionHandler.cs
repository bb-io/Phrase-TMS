namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobDeletionHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "JOB_DELETED";

    public JobDeletionHandler() : base(SubscriptionEvent) { }
}