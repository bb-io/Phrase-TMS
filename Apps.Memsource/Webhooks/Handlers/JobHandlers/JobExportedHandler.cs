namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobExportedHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "JOB_EXPORTED";

    public JobExportedHandler() : base(SubscriptionEvent) { }
}