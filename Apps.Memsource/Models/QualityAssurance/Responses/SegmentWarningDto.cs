using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses
{
    public class SegmentWarningDto
    {
        [Display("Segment ID")]
        public string? segmentId { get; set; }
        
        public warning? Warnings { get; set; }
        } 
    
    public class warning
    {
        public string? Type { get; set; }

        public bool? Ignored { get; set; }
    }

}
