using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTms.Models.Jobs.Responses;

public class JobResponse
{
    [Display("Job ID")] public string Uid { get; set; }

    [Display("Project ID")] public string ProjectUid { get; set; }

    [Display("File name")] public string Filename { get; set; }

    public string Status { get; set; }

    [Display("Target language")] public string TargetLanguage { get; set; }

    //[Display("Due date")] public string DateDue { get; set; }
}