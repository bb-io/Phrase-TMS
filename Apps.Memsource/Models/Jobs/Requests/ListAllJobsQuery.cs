using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class ListAllJobsQuery
{
    [Display("Count")]
    public bool? Count { get; set; }

    [Display("Workflow level")]
    public int? WorkflowLevel { get; set; }

    [Display("Assigned user")]
    public int? AssignedUser { get; set; }

    [Display("Due in hours")]
    public int? DueInHours { get; set; }

    [Display("File name")]
    public string? Filename { get; set; }

    [Display("Target language")]
    public string? TargetLang { get; set; }

    [Display("Assigned vendor")]
    public int? AssignedVendor { get; set; }
}