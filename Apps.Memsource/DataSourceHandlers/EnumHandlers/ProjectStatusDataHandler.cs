using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class ProjectStatusDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"ACCEPTED_BY_VENDOR", "Accepted by vendor"},
        {"ASSIGNED", "Assigned"},
        {"CANCELLED", "Cancelled"},
        {"COMPLETED", "Completed"},
        {"COMPLETED_BY_VENDOR", "Completed by vendor"},
        {"DECLINED_BY_VENDOR", "Declined by vendor"},
        {"NEW", "New"},
    };
}