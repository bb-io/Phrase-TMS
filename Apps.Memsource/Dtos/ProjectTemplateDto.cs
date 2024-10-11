using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class ProjectTemplateDto
{
    [Display("Template name")] public string TemplateName { get; set; }
    [Display("Project template ID")] public string UId { get; set; }
    [Display("Creation date")] public DateTime DateCreated { get; set; }
}