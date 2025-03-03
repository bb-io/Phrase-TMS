using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class FormatDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("CSV", "CSV"),
        new DataSourceItem("CSV_EXTENDED", "CSV Extended"),
        new DataSourceItem("JSON", "JSON"),
        new DataSourceItem("LOG", "LOG"),
    };
}