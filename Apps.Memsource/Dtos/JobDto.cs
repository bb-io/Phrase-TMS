using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class JobDto
{
    [Display("UID")] public string Uid { get; set; }

    [Display("File name")] public string Filename { get; set; }

    public string Status { get; set; }

    [Display("Inner ID", Description = "InnerId is a sequential number of a job in a project.\nJobs created from the same file share the same innerId across workflow steps.")]
    public string InnerId { get; set; }

    [Display("Target language")] 
    public string TargetLang { get; set; }

    public ProjectDto Project { get; set; }
}