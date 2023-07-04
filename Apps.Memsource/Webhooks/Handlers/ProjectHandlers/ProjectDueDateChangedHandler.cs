namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers
{
    public class ProjectDueDateChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "PROJECT_DUE_DATE_CHANGED";

        public ProjectDueDateChangedHandler() : base(SubscriptionEvent) { }
    }
}
