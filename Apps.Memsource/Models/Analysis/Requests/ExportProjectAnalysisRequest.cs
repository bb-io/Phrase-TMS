using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class ExportProjectAnalysisRequest
{
    [Display("Calculate synthetic MT bucket", Description = 
        "If true, identifies words translated by MT below the TM threshold " +
        "and moves them to a 'machine_translated' bucket.")]
    public bool? CalculateSyntheticMtBucket { get; set; }
}