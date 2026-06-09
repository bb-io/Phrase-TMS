using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.InteroperableXliff;

public class DownloadInteroperableXliffResponse
{
    [Display("File")]
    public FileReference File { get; set; } = new();

    [Display("Total segments")]
    public int TotalSegments { get; set; }

    [Display("Locked segments")]
    public int LockedSegments { get; set; }
    
    [Display("Confirmed segments")]
    public int ConfirmedSegments { get; set; }
    
    [Display("Segments with empty targets")]
    public int EmptyTargetSegments { get; set; }

    [Display("Source locale")]
    public string SourceLocale { get; set; } = string.Empty;

    [Display("Target locale")]
    public string TargetLocale { get; set; } = string.Empty;
}
