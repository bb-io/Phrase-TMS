using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class UpdateTmRequest
{
    [Display("Translation memory ID")]
    [DataSource(typeof(TmDataHandler))]
    public string TranslationMemoryUId { get; set; } = string.Empty;
    
    [Display("XLIFF file")]
    public FileReference File { get; set; } = null!;

    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? TargetLanguage { get; set; }

    [Display("Segment state to import", Description = "State of the segments to be imported into the translation memory, by default all existing translations will be sent to TM.")]
    [StaticDataSource(typeof(XliffStateDataSourceHandler))]
    public IEnumerable<string>? SegmentStates { get; set; }
}