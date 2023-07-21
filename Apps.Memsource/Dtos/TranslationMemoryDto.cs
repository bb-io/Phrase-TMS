using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos
{
    public class TranslationMemoryDto
    {
        [Display("UID")]
        public string UId { get; set; }

        public string Name { get; set; }

        [Display("Source language")]
        public string SourceLang { get; set; }

        [Display("Target languages")]
        public IEnumerable<string> TargetLangs { get; set; }
    }
}
