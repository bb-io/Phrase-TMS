using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class ImportTmxRequest
    {
        [Display("Translation memory UID")]
        public string TranslationMemoryUId { get; set; }

        public byte[] File { get; set; }

        [Display("File name")]
        public string FileName { get; set; }
    }
}
