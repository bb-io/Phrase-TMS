using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class SegmentDto
{
    [Display("Segment ID")]
    public string Id { get; set; }

    public string Source { get; set; }

    public string Target { get; set; }
}