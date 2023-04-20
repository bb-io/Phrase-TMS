using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobDeletionHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_DELETED";

        public JobDeletionHandler() : base(SubscriptionEvent) { }
    }
}
