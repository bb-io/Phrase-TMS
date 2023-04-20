using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers
{
    public class ProjectDueDateChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "PROJECT_DUE_DATE_CHANGED";

        public ProjectDueDateChangedHandler() : base(SubscriptionEvent) { }
    }
}
