using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Handlers.OtherHandlers
{
    public class AnalysisCreationHandler : BaseWebhookHandler
    {

        const string SubscriptionEvent = "ANALYSIS_CREATED";

        public AnalysisCreationHandler() : base(SubscriptionEvent) { }
    }
}
