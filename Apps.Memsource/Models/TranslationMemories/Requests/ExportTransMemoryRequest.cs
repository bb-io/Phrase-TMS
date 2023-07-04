namespace Apps.PhraseTMS.Models.TranslationMemories.Responses
{
    public class ExportTransMemoryRequest
    {
        public string TranslationMemoryUId { get; set; }

        public string FileFormat { get; set; } //"TMX" "XLSX"
    }
}
