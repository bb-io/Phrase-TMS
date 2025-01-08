using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class JobStatusChangedRequest
{
    [Display("Statuses"), StaticDataSource(typeof(JobStatusDataHandler))]
    public IEnumerable<string>? Status { get; set; }
}