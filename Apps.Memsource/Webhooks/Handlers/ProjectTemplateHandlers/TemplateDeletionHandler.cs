using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectTemplateHandlers
{
    public class TemplateDeletionHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "PROJECT_TEMPLATE_DELETED";

        public TemplateDeletionHandler() : base(SubscriptionEvent) { }
    }
}
