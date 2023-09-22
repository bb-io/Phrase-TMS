namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;

public class ProjectDeletionHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "PROJECT_DELETED";

    public ProjectDeletionHandler() : base(SubscriptionEvent) { }
}