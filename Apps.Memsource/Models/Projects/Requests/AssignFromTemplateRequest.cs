using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class AssignFromTemplateRequest : ProjectRequest
    {
        [Display("Template UID")]
        [DataSource(typeof(ProjectTemplateDataHandler))]
        public string TemplateUId { get; set; }

        [Display("Job UIDs")]
        [DataSource(typeof(JobDataHandler))]
        public IEnumerable<string>? JobsUIds { get; set; }
    }
}
