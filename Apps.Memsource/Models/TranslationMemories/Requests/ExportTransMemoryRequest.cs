using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class ExportTransMemoryRequest
    {
        [Display("Translation memory UID")]
        public string TranslationMemoryUId { get; set; }

        [Display("File format")]
        public string FileFormat { get; set; } //"TMX" "XLSX"
    }
}
