using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobContinuousUpdatedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "CONTINUOUS_JOB_UPDATED";

        public JobContinuousUpdatedHandler() : base(SubscriptionEvent) { }
    }
}
