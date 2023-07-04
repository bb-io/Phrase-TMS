namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers
{
    public class ProjectStatusChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "PROJECT_STATUS_CHANGED";

        public ProjectStatusChangedHandler() : base(SubscriptionEvent) { }
    }
}
