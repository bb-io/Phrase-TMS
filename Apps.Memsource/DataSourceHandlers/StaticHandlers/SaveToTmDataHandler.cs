using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers
{
    public class SaveToTmDataHandler : IStaticDataSourceHandler
    {
            public Dictionary<string, string> GetData() => new()
        {
            { "All", "All segments" },
            { "Confirmed", "Confirmed segments" },
            { "None", "None" }
        };
    }
}
