

using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers
{
    public class BilingualFormatDataHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
        {
            new DataSourceItem("MXLF", ".mxliff"),
            new DataSourceItem("DOCX", ".docx"),
            new DataSourceItem("TMX", ".tmx"),
            new DataSourceItem("XLIFF", ".xlf"),
        };
    }
}
