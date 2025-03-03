using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Jobs;

public class JobDto
{
    [Display("Job ID")] 
    public string Uid { get; set; }

    [Display("Inner ID", Description = "InnerId is a sequential number of a job in a project.\nJobs created from the same file share the same innerId across workflow steps.")]
    public string InnerId { get; set; }

    public string Status { get; set; }

    [Display("Providers")]
    public IEnumerable<ProviderDto>? providers { get; set; }

    [Display("Target language")]
    public string TargetLang { get; set; }

    [Display("Source language")]
    public string SourceLang { get; set; }

    [Display("Workflow step")]
    [JsonProperty("workflowStep")]
    public WorkflowStepDto WorkflowStep { get; set; }

    [Display("File name")]
    public string Filename { get; set; }

    [Display("Due date")]
    [JsonProperty("dateDue")]
    public DateTime? DateDue { get; set; }

    [Display("Word count")]
    public int WordsCount { get; set; }

    [Display("Is parent job split")]
    public bool IsParentJobSplit { get; set; }

    [Display("Update source date")]
    [JsonProperty("updateSourceDate")]
    public DateTime? UpdateSourceDate { get; set; }

    [Display("Update target date")]
    [JsonProperty("updateTargetDate")]
    public DateTime? UpdateTargetDate { get; set; }

    [Display("Created date")]
    [JsonProperty("dateCreated")]
    public DateTime DateCreated { get; set; }

    public Project Project { get; set; }

    [Display("Last workflow level")]
    public int LastWorkflowLevel { get; set; }

    [Display("Imported")]
    [JsonProperty("imported")]
    public bool Imported { get; set; }

    [Display("Continuous")]
    [JsonProperty("continuous")]
    public bool Continuous { get; set; }

    [Display("Server task ID")]
    [JsonProperty("serverTaskId")]
    public string ServerTaskId { get; set; }
}

public class Project
{
    [Display("Project ID")]
    public string UId { get; set; }

    [Display("Name")]
    public string Name { get; set; }
}