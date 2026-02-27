using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers
{
    public class QuoteTypeDataHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
        {
            new DataSourceItem("BUYER", "Buyer"),
            new DataSourceItem("PROVIDER", "Provider")
        };
    }
}
