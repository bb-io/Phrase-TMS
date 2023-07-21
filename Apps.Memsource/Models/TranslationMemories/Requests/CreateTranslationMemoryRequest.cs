using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class CreateTranslationMemoryRequest
    {
        public string Name { get; set; }

        [Display("Source language")]
        public string SourceLang { get; set; }

        [Display("Target languages")]
        public string[] TargetLang { get; set; }
        
        public string? Note { get; set; }
    }
}
