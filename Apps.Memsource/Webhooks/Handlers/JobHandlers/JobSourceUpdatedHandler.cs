﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobSourceUpdatedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_UPDATED";

        public JobSourceUpdatedHandler() : base(SubscriptionEvent) { }
    }
}
