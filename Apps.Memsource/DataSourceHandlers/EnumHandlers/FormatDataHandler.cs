using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class FormatDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "CSV", "csv" },
        { "CSV_EXTENDED", "csv extended" },
        { "JSON", "json" },
        { "LOG", "log" }
    };
}