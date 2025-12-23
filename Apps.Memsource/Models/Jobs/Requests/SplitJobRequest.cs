using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class SplitJobRequest
    {
        [Display("Segment ordinals", Description = "After specific segments. Provide segment ordinals to split after (e.g., 5 means split after segment #5).")]
        public IEnumerable<string>? SegmentOrdinals { get; set; }

        [Display("Part count", Description = "Into X parts. Total number of parts to split into.")]
        public string? PartCount { get; set; }

        [Display("Part size", Description = "Into parts with specific size. Number of segments per part.")]
        public string? PartSize { get; set; }

        [Display("Word count", Description = "Into parts with specific word count.")]
        public string? WordCount { get; set; }

        [Display("By document parts", Description = "By document parts (PowerPoint only).")]
        public bool? ByDocumentParts { get; set; }
    }
}
