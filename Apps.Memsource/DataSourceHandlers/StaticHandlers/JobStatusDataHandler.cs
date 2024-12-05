using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class JobStatusDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "NEW", "New" },
        { "EMAILED", "Emailed to provider" },
        { "ACCEPTED", "Accepted by provider" },
        { "DECLINED_BY_LINGUIST", "Declined by provider" },
        { "COMPLETED_BY_LINGUIST", "Completed by provider" },
        { "COMPLETED", "Delivered" },
        { "CANCELLED", "Cancelled" },
    };
}