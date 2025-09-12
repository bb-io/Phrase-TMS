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
    public class ConversationRequest
    {
        [Display("Conversation ID")]
        [DataSource(typeof(ConversationDataHandler))]
        public string ConversationUId { get; set; }
    }
}
