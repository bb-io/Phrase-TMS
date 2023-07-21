using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class DeleteTransMemoryRequest
    {
        [Display("Translation memory UID")] public string TranslationMemoryUId { get; set; }

        public bool? Purge { get; set; }
    }
}