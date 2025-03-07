using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class TmFormatDataHandler : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("TMX", "TMX"),
        new DataSourceItem("XLSX", "XLSX"),
    };
}