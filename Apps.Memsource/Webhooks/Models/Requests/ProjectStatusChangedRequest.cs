using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class ProjectStatusChangedRequest
{
    [StaticDataSource(typeof(ProjectStatusDataHandler))]
    public string? Status { get; set; }
}