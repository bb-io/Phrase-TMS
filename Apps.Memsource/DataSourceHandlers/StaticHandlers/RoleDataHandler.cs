using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class RoleDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("ADMIN", "Admin"),
        new DataSourceItem("PROJECT_MANAGER", "Project manager"),
        new DataSourceItem("LINGUIST", "Linguist"),
        new DataSourceItem("GUEST", "Guest"),
        new DataSourceItem("SUBMITTER", "Submitter"),
        new DataSourceItem("PORTAL_MEMBER", "Portal member"),
    };
}