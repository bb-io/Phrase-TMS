using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class SortDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("DATE_ARCHIVED", "Archived date"),
        new DataSourceItem("DATE_CREATED", "Created date"),
        new DataSourceItem("INTERNAL_ID", "Internal ID"),
        new DataSourceItem("NAME", "Name"),

    };
}