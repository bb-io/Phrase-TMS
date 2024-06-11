using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests;

public class AddIgnoredWarningRequest
{
    [Display("Segment UID")]
    [DataSource(typeof(SegmentDataHandler))]
    public string SegmentUId { get; set; }

    [Display("Warning ID")] 
    public string WarningId { get; set; }
}