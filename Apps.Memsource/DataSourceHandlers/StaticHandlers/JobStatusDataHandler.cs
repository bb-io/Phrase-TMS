using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class JobStatusDataHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> Data => new()
    {
        { "NEW", "New" },
        { "EMAILED", "Emailed to provider" },
        { "ACCEPTED", "Accepted by provider" },
        { "DECLINED", "Declined by provider" },
        { "COMPLETED_BY_LINGUIST", "Completed by provider" },
        { "COMPLETED", "Delivered" },
        { "CANCELLED", "Cancelled" },
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}