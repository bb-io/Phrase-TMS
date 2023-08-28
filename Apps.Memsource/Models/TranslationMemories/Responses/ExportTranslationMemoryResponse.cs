using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.PhraseTMS.Models.TranslationMemories.Responses
{
    public class ExportTranslationMemoryResponse
    {
        [Display("File")]
        public File File { get; set; }
    }
}
