using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Responses;

public class JobResponse
{
    [Display("Job ID")] 
    public string Uid { get; set; } = string.Empty;

    [Display("Project ID")] 
    public string ProjectUid { get; set; } = string.Empty;

    [Display("Project name")]
    public string ProjectName { get; set; } = string.Empty;

    [Display("File name")] public string Filename { get; set; }

    public string Status { get; set; } = string.Empty;

    [Display("Target language")] 
    public string TargetLanguage { get; set; } = string.Empty;

    [Display("Source language")] 
    public string SourceLanguage { get; set; } = string.Empty;

    [Display("Word count")] 
    public int WordCount { get; set; }

    [Display("Assigned to")] 
    public IEnumerable<UserDto> AssignedTo { get; set; } = default!;

    //[Display("Due date")] public string DateDue { get; set; }
}