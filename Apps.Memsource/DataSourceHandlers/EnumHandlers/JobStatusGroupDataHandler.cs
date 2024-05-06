using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;

public class JobStatusGroupDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "ACCEPTED", "Accepted" },
        { "COMPLETED", "Completed" },
        { "NEW", "New" },
    };
}