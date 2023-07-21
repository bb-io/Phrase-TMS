using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class GetTranslationMemoryRequest
    {
        [Display("Translation memory UID")]
        public string TranslationMemoryUId { get; set; }
    }
}
