using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests;

public class AddIgnoredWarningRequest
{
    [Display("Segment")]
    [DataSource(typeof(SegmentDataHandler))]
    public string SegmentUId { get; set; }

    [Display("Warning ID")] 
    public string WarningId { get; set; }
}