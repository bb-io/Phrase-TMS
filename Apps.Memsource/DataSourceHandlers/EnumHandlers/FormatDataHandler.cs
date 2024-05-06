using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class FormatDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "CSV", "CSV" },
        { "CSV_EXTENDED", "CSV Extended" },
        { "JSON", "JSON" },
        { "LOG", "LOG" }
    };
}