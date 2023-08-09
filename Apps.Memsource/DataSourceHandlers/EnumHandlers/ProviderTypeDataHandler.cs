using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class ProviderTypeDataHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        {"USER", "User"},
        {"VENDOR", "Vendor"}
    };
}