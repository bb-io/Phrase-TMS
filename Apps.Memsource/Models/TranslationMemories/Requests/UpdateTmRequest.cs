using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class UpdateTmRequest
{
    [Display("Translation memory ID"), DataSource(typeof(TmDataHandler))]
    public string TranslationMemoryUId { get; set; } = string.Empty;
    
    [Display("XLIFF file")]
    public FileReference File { get; set; } = null!;
    
    
    [Display("Target language"), DataSource(typeof(LanguageDataHandler))]
    public string? TargetLanguage { get; set; }
}