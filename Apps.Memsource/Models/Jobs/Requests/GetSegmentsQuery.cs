using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class GetSegmentsQuery
{
    [Display("Begin index")]
    public int? BeginIndex { get; set; }

    [Display("End index")]
    public int? EndIndex { get; set; }
}