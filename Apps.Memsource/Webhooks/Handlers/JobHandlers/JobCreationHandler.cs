namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobCreationHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_CREATED";

        public JobCreationHandler() : base(SubscriptionEvent) { }
    }
}
