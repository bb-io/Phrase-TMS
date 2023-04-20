using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers
{
    public class ProjectStatusChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "PROJECT_STATUS_CHANGED";

        public ProjectStatusChangedHandler() : base(SubscriptionEvent) { }
    }
}
