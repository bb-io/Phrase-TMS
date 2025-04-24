using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Jobs.Requests;
public class WorkflowStepOptionalRequest
{
    [Display("Workflow step ID")]
    [DataSource(typeof(WorkflowStepDataHandler))]
    public string? WorkflowStepId { get; set; }
}
