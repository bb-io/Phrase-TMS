using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class ProjectStatusChangedRequest
{
    [Display("Statuses", Description =
             "The event will be triggered if the project changes its status to one of the specified statuses."), StaticDataSource(typeof(ProjectStatusDataHandler))]
    public IEnumerable<string>? Status { get; set; }
}