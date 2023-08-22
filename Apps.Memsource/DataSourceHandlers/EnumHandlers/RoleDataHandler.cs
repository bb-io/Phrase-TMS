using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class RoleDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"ADMIN", "Admin"},
        {"PROJECT_MANAGER", "Project manager"},
        {"LINGUIST", "Linguist"},
        {"GUEST", "Guest"},
        {"SUBMITTER", "Submitter"},
    };
}