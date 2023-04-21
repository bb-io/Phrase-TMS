﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.JobHandlers
{
    public class JobTargetUpdatedHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "JOB_TARGET_UPDATED";

        public JobTargetUpdatedHandler() : base(SubscriptionEvent) { }
    }
}