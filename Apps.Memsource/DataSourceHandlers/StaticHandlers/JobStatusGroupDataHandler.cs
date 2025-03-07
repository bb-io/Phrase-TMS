using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class JobStatusGroupDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("ACCEPTED", "Accepted"),
        new DataSourceItem("COMPLETED", "Completed"),
        new DataSourceItem("NEW", "New"),
    };
}