using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class TmFormatDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"TMX", "TMX"},
        {"XLSX", "XLSX"}
    };
}