using Apps.PhraseTms.Dtos;

namespace Apps.PhraseTms.Models.Jobs.Responses
{
    public class GetSegmentsResponse
    {
        public IEnumerable<SegmentDto> Segments { get; set; }
    }
}
