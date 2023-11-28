using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

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

    [Display("File")]
    [DataSource(typeof(FileNameDataHandler))]
    public string? Filename { get; set; }

    [Display("Target language")]
    [DataSource(typeof(JobTargetLanguagesDataHandler))]
    public string? TargetLang { get; set; }

    [Display("Assigned vendor")]
    public int? AssignedVendor { get; set; }
}