﻿using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectTemplateHandlers;

public class TemplateCreationHandler(InvocationContext invocationContext)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent)
{
    const string SubscriptionEvent = "PROJECT_TEMPLATE_CREATED";
}