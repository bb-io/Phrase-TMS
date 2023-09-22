using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class CreateTranslationMemoryRequest
{
    public string Name { get; set; }

    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLang { get; set; }

    [Display("Target languages")]
    public string[] TargetLang { get; set; }
        
    public string? Note { get; set; }
}