using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class GetSegmentsResponse
    {
        public IEnumerable<SegmentDto> Segments { get; set; }
    }
}
