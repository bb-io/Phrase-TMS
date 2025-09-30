using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class InsertSegmentRequest
{
    [Display("Translation memory ID")]
    [DataSource(typeof(TmDataHandler))]
    public string TranslationMemoryUId { get; set; } = string.Empty;

    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string TargetLanguage { get; set; } = string.Empty;

    [Display("Source segment")]
    public string SourceSegment { get; set; } = string.Empty;

    [Display("Target segment")]
    public string TargetSegment { get; set; } = string.Empty;

    [Display("Previous source segment")]
    public string? PreviousSourceSegment { get; set; } = string.Empty;

    [Display("Next source segment")]
    public string? NextSourceSegment { get; set; } = string.Empty;
}