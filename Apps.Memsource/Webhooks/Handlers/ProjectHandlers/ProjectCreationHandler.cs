namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;

public class ProjectCreationHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "PROJECT_CREATED";

    public ProjectCreationHandler() : base(SubscriptionEvent) { }
}