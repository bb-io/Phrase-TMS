using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class JobStatusGroupDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"ACCEPTED", "Accepted"},
        {"COMPLETED", "Completed"},
        {"NEW", "New"},
    };
}