namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers
{
    public class ProjectSharedAssignedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "SHARED_PROJECT_ASSIGNED";

        public ProjectSharedAssignedHandler() : base(SubscriptionEvent) { }
    }
}
