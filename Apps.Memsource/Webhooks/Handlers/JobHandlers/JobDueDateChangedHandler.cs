using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobDueDateChangedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_DUE_DATE_CHANGED";

        public JobDueDateChangedHandler() : base(SubscriptionEvent) { }
    }
}
