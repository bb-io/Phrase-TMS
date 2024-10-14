using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class JobStatusDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "ACCEPTED", "Accepted" },
        { "CANCELLED", "Cancelled" },
        { "COMPLETED", "Completed" },
        { "COMPLETED_BY_LINGUIST", "Completed by linguist" },
        { "DECLINED", "Declined" },
        { "DELIVERED", "Delivered" },
        { "EMAILED", "Emailed" },
        { "NEW", "New" },
        { "REJECTED", "Rejected" },
    };
}