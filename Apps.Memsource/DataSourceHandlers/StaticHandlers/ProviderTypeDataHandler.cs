using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class ProviderTypeDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "USER", "User" },
        { "VENDOR", "Vendor" }
    };
}