using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class InsertSegmentRequest
{
    [Display("Translation memory UID")]
    [DataSource(typeof(TmDataHandler))]
    public string TranslationMemoryUId { get; set; }

    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string TargetLanguage { get; set; }

    [Display("Source segment")]
    public string SourceSegment { get; set; }

    [Display("Target segment")]
    public string TargetSegment { get; set; }
        
    [Display("Previous source segment")]
    public string? PreviousSourceSegment { get; set; }

    [Display("Next source segment")]
    public string? NextSourceSegment { get; set; }
}