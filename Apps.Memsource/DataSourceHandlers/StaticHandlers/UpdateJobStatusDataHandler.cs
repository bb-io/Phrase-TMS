using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class UpdateJobStatusDataHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> Data => new()
    {
        { "ACCEPTED", "Accepted" },
        { "CANCELLED", "Cancelled" },
        { "COMPLETED", "Completed" },
        { "DECLINED", "Declined" },
        { "DELIVERED", "Delivered" },
        { "EMAILED", "Emailed" },
        { "NEW", "New" },
        { "REJECTED", "Rejected" },
    };

    public IEnumerable<DataSourceItem> GetData()
    {
        return Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}