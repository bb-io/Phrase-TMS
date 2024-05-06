using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class ProjectStatusDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "ACCEPTED_BY_VENDOR", "Accepted by vendor" },
        { "ASSIGNED", "Assigned" },
        { "CANCELLED", "Cancelled" },
        { "COMPLETED", "Completed" },
        { "COMPLETED_BY_VENDOR", "Completed by vendor" },
        { "DECLINED_BY_VENDOR", "Declined by vendor" },
        { "NEW", "New" },
    };
}