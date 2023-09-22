namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers;

public class JobAssignedHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "JOB_ASSIGNED";

    public JobAssignedHandler() : base(SubscriptionEvent) { }
}