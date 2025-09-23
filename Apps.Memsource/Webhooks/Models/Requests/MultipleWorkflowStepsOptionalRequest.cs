using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Webhooks.Models.Requests
{
    public class MultipleWorkflowStepsOptionalRequest
    {
        [Display("Workflow step IDs")]
        [DataSource(typeof(WorkflowStepDataHandler))]
        public IEnumerable<string>? WorkflowStepIds { get; set; }
    }
}
