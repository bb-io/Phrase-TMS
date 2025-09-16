using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos
{
    public class AggregatedSegmentsCountsResultDto
    {
        [Display("Segments count")]
        public double SegmentsCount { get; set; }

        [Display("Completed segments count")]
        public double CompletedSegmentsCount { get; set; }

        [Display("Locked segments count")]
        public double LockedSegmentsCount { get; set; }

        [Display("Translated segments count")]
        public double TranslatedSegmentsCount { get; set; }

        [Display("Words count")]
        public double WordsCount { get; set; }

        [Display("All segments are confirmed")]
        public bool AllConfirmed { get; set; }
    }
}
