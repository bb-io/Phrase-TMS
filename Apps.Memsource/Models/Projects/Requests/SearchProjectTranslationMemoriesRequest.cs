using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class SearchProjectTranslationMemoriesRequest
{
    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? TargetLang { get; set; }

    [Display("Workflow step UID")]
    [DataSource(typeof(WorkflowStepDataHandler))]
    public string? WorkflowStepUid { get; set; }

    [Display("Write mode")]
    public bool? WriteMode { get; set; }
}
