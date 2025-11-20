using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class ProjectTemplateDto
{
    [Display("Template name")] public string TemplateName { get; set; }
    [Display("Project template ID")] public string UId { get; set; }
    [Display("Creation date")] public DateTime DateCreated { get; set; }
    public string? Note { get; set; }
    [Display("Target languages")] public IEnumerable<string>? TargetLangs { get; set; }
    [Display("Source language")]  public string? SourceLang { get; set; }

    [Display("Workflow steps")]
    public IEnumerable<ProjectTemplateWorkflowStepDto>? WorkflowSteps { get; set; }
}
public class ProjectTemplateWorkflowStepDto
{
    [Display("Workflow step UID")]
    public string? Uid { get; set; }

    public string? Id { get; set; }

    [Display("Workflow step name")]
    public string? Name { get; set; }
}