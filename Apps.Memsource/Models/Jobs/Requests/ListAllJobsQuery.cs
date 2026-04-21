using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class ListAllJobsQuery
{
    [Display("Assigned users ID (not UID)"), JsonProperty("assignedUser")]
    [DataSource(typeof(UserLegacyIdDataHandler))]
    public IEnumerable<int>? AssignedUsers { get; set; }

    [Display("Due in hours"), JsonProperty("dueInHours")]
    public int? DueInHours { get; set; }

    [Display("File name"), JsonProperty("filename")]
    public string? Filename { get; set; }

    [Display("Target language"), JsonProperty("targetLang")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? TargetLang { get; set; }

    [Display("Assigned vendor ID (not UID)"), JsonProperty("assignedVendor")]
    public int? AssignedVendor { get; set; }

    [Display("Project note contains", Description ="Checks the project note")]
    public string? NoteContains { get; set; }

    [Display("Project note doesn’t contain", Description = "Checks the project note")]
    public string? NoteNotContains { get; set; }
}