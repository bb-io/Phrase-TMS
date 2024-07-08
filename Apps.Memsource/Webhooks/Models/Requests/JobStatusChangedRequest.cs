using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class JobStatusChangedRequest
{
    [StaticDataSource(typeof(JobStatusDataHandler))]
    public string? Status { get; set; }
}