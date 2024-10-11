using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Webhooks.Models.Requests
{
    public class OptionalJobRequest
    {
        [Display("Job ID")]
        [DataSource(typeof(JobDataHandler))]
        public string? JobUId { get; set; }
    }
}
