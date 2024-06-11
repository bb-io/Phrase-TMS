

using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers
{
    public class BilingualFormatDataHandler : IStaticDataSourceHandler
    {
        public Dictionary<string, string> GetData() => new()
    {
        { "MXLF", ".mxliff" },
        { "DOCX", ".docx" },
        { "TMX", ".tmx" },
        { "XLIFF", ".xlf" }
    };
    }
}
