using Apps.PhraseTms;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Webhooks.Handlers.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers
{
    public class ProjectCreationHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "PROJECT_CREATED";

        public ProjectCreationHandler() : base(SubscriptionEvent) { }
    }
}
