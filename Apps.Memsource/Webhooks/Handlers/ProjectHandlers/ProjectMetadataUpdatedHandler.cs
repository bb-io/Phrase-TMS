namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers;

public class ProjectMetadataUpdatedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "PROJECT_METADATA_UPDATED";

    public ProjectMetadataUpdatedHandler() : base(SubscriptionEvent) { }
}