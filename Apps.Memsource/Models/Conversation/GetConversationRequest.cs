using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Conversation
{
    public class GetConversationRequest
    {
        [Display("Job ID")]
        [DataSource(typeof(JobDataHandler))]
        public string JobUId { get; set; }

        [Display("Conversation ID")]
        public string ConversationUId { get; set; }
    }
}
