namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobDueDateChangedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_DUE_DATE_CHANGED";

        public JobDueDateChangedHandler() : base(SubscriptionEvent) { }
    }
}
