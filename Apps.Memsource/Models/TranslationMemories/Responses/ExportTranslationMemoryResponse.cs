using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;


namespace Apps.PhraseTMS.Models.TranslationMemories.Responses;

public class ExportTranslationMemoryResponse
{
    [Display("File")]
    public FileReference File { get; set; }
}