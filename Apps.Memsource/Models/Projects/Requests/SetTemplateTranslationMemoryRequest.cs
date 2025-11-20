using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class SetTemplateTranslationMemoryRequest
    {
        [Display("Translation memory UID")]
        [DataSource(typeof(TmDataHandler))]
        public string TransMemoryUid { get; set; } = default!;

        [Display("Workflow step UID")]
        [DataSource(typeof(WorkflowStepDataHandler))]
        public string WorkflowStepUid { get; set; } = default!;
    }
}
