using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class ListAllJobsQuery
{
    [Display("Count"), JsonProperty("count")]
    public bool? Count { get; set; }

    [Display("Workflow level"), JsonProperty("workflowLevel")]
    public int? WorkflowLevel { get; set; }

    [Display("Assigned user"), JsonProperty("assignedUser")]
    public int? AssignedUser { get; set; }

    [Display("Due in hours"), JsonProperty("dueInHours")]
    public int? DueInHours { get; set; }

    [Display("File"), JsonProperty("filename")]
    [DataSource(typeof(FileNameDataHandler))]
    public string? Filename { get; set; }

    [Display("Target language"), JsonProperty("targetLang")]
    [DataSource(typeof(JobTargetLanguagesDataHandler))]
    public string? TargetLang { get; set; }

    [Display("Assigned vendor"), JsonProperty("assignedVendor")]
    public int? AssignedVendor { get; set; }
}