using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class JobStatusDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"ACCEPTED", "Accepted"},
        {"CANCELLED", "Cancelled"},
        {"COMPLETED", "Completed"},
        {"DECLINED", "Declined"},
        {"DELIVERED", "Delivered"},
        {"EMAILED", "Emailed"},
        {"NEW", "New"},
        {"REJECTED", "Rejected"},
    };
}