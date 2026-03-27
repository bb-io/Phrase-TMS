using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Responses;

public class LockSegmentsFromMxliffResponse
{
    [Display("Source locked segments count")]
    public int SourceLockedSegmentsCount { get; set; }

    [Display("Applied locked segments count")]
    public int AppliedLockedSegmentsCount { get; set; }
}
