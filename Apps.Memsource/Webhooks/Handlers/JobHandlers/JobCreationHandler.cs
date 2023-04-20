using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobCreationHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_CREATED";

        public JobCreationHandler() : base(SubscriptionEvent) { }
    }
}
