namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobContinuousUpdatedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "CONTINUOUS_JOB_UPDATED";

        public JobContinuousUpdatedHandler() : base(SubscriptionEvent) { }
    }
}
