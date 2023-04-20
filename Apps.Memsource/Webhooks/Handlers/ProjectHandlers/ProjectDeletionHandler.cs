using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.ProjectHandlers
{
    public class ProjectDeletionHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "PROJECT_DELETED";

        public ProjectDeletionHandler() : base(SubscriptionEvent) { }
    }
}
