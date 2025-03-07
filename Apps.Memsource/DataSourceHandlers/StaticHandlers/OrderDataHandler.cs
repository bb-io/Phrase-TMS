using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class OrderDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("ASC", "Ascending"),
        new DataSourceItem("DESC", "Descending"),
    };
}