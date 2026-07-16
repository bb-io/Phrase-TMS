using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.WorkflowStep.Request;

public class WorkflowStepRequest
{
    [Display("Workflow ID"), DataSource(typeof(WorkflowStepDataHandler))]
    public string WorkflowStepId { get; set; } = string.Empty;
}