using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class FormatDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "CSV", "CSV" },
        { "CSV_EXTENDED", "CSV Extended" },
        { "JSON", "JSON" },
        { "LOG", "LOG" }
    };
}