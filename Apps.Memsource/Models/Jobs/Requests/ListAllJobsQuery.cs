using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class ListAllJobsQuery
{
    [DefinitionIgnore, JsonProperty("workflowLevel")]
    public int? WorkflowLevel { get; set; }

    [Display("Assigned user ID"), JsonProperty("assignedUser")]
    public int? AssignedUser { get; set; }

    [Display("Due in hours"), JsonProperty("dueInHours")]
    public int? DueInHours { get; set; }

    [Display("File name"), JsonProperty("filename")]
    public string? Filename { get; set; }

    [Display("Target language"), JsonProperty("targetLang")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? TargetLang { get; set; }

    [Display("Assigned vendor ID"), JsonProperty("assignedVendor")]
    public int? AssignedVendor { get; set; }
}