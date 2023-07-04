namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobUnexportedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_UNEXPORTED";

        public JobUnexportedHandler() : base(SubscriptionEvent) { }
    }
}
