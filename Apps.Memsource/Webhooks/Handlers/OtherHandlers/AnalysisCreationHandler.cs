namespace Apps.PhraseTMS.Webhooks.Handlers.OtherHandlers;

public class AnalysisCreationHandler : BaseWebhookHandler
{

    const string SubscriptionEvent = "ANALYSIS_CREATED";

    public AnalysisCreationHandler() : base(SubscriptionEvent) { }
}