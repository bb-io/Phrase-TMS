using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class JobStatusesRequest
{
    [Display("Statuses"), StaticDataSource(typeof(JobStatusDataHandler))]
    public IEnumerable<string>? Statuses { get; set; }
}