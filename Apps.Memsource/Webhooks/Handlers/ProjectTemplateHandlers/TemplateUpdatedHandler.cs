namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectTemplateHandlers
{
    public class TemplateUpdatedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "PROJECT_TEMPLATE_UPDATED";

        public TemplateUpdatedHandler() : base(SubscriptionEvent) { }
    }
}
