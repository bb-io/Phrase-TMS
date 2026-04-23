using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class UpdateSegmentStatesRequest
{
    [Display("Confirm segments with state")]
    [StaticDataSource(typeof(XliffStateDataSourceHandler))]
    public string? ConfirmSegmentsWithState { get; set; }

    [Display("Lock segments with state")]
    [StaticDataSource(typeof(XliffStateDataSourceHandler))]
    public string? LockSegmentsWithState { get; set; }
}
