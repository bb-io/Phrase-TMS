using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers
{
    public class ConversationStatusDataHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
        {
            new DataSourceItem("resolved", "Resolved"),
            new DataSourceItem("unresolved", "Unresolved"),
        };
    }
}