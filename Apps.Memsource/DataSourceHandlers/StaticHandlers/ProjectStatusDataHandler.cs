using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class ProjectStatusDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("ACCEPTED_BY_VENDOR", "Accepted by vendor"),
        new DataSourceItem("ASSIGNED", "Assigned"),
        new DataSourceItem("CANCELLED", "Cancelled"),
        new DataSourceItem("COMPLETED", "Completed"),
        new DataSourceItem("COMPLETED_BY_VENDOR", "Completed by vendor"),
        new DataSourceItem("DECLINED_BY_VENDOR", "Declined by vendor"),
        new DataSourceItem("NEW", "New"),
    };
}