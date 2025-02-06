using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class SortDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "DATE_ARCHIVED", "Archived date" },
        { "DATE_CREATED", "Created date" },
        { "INTERNAL_ID", "Internal ID" },
        { "NAME", "Name" }

    };
}