using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class SetTemplateTermBasesRequest
    {
        [Display("Term base ID")]
        [DataSource(typeof(TermBaseDataHandler))]
        public string TermBaseId { get; set; } = default!;

        [Display("Target language")]
        [DataSource(typeof(LanguageDataHandler))]
        public string TargetLang { get; set; } = default!;

        [Display("Workflow step ID")]
        [DataSource(typeof(WorkflowStepDataHandler))]
        public string WorkflowStepId { get; set; } = default!;
    }
}
