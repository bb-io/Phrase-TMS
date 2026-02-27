using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers
{
    public class QuoteStatusDataHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
        {
            new DataSourceItem("APPROVED", "Approved"),
            new DataSourceItem("DECLINED", "Declined"),
            new DataSourceItem("DRAFT", "Draft"),
            new DataSourceItem("FOR_APPROVAL", "For approval"),
            new DataSourceItem("NEW", "New")
        };
    }
}
