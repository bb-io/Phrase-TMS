using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class RoleDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "ADMIN", "Admin" },
        { "PROJECT_MANAGER", "Project manager" },
        { "LINGUIST", "Linguist" },
        { "GUEST", "Guest" },
        { "SUBMITTER", "Submitter" },
    };
}