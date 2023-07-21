using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class InsertSegmentRequest
    {
        [Display("Translation memory UID")]
        public string TranslationMemoryUId { get; set; }

        [Display("Target language")]
        public string TargetLanguage { get; set; }

        [Display("Source segment")]
        public string SourceSegment { get; set; }

        [Display("Target segment")]
        public string TargetSegment { get; set; }
        
        [Display("Previous source segment")]
        public string? PreviousSourceSegment { get; set; }

        [Display("Next source segment")]
        public string? NextSourceSegment { get; set; }
    }
}
