namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobSourceUpdatedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_UPDATED";

        public JobSourceUpdatedHandler() : base(SubscriptionEvent) { }
    }
}
