namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectTemplateHandlers;

public class TemplateCreationHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "PROJECT_TEMPLATE_CREATED";

    public TemplateCreationHandler() : base(SubscriptionEvent) { }
}