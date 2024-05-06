using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class TmFormatDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "TMX", "TMX" },
        { "XLSX", "XLSX" }
    };
}