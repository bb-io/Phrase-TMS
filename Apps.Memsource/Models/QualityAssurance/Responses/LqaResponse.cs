using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class LqaResponse
{
    [Display("Overall feedback")]
    public string OverallFeedback { get; set; } = default!;

    [Display("Report can be downloaded")]
    public bool ReportCanBeDownloaded { get; set; }
    
    [Display("LQA status")]
    public string Status { get; set; } = default!;

    public string Availability { get; set; } = default!;

    [Display("Requested job part ID")]
    public string RequestedJobPartUid { get; set; } = default!;

    [Display("Assessment job part ID")]
    public string AssessmentJobPartUid { get; set; } = default!;

    [Display("Started at")]
    
    public DateTime? StartedDate { get; set; }
    
    [Display("Finished at")]
    public DateTime? FinishedDate { get; set; }
}