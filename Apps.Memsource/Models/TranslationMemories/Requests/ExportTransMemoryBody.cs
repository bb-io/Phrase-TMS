using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class ExportTransMemoryBody
{
    [Display("Export Target Languages")]
    public IEnumerable<string>? ExportTargetLangs { get; set; }

    [Display("Callback URL")]
    public string? CallbackUrl { get; set; }
}