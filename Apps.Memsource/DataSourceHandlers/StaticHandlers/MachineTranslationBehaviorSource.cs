using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;

public class MachineTranslationBehaviorSource : IStaticDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
    {
        new DataSourceItem("APPLY_MT_ABOVE_THRESHOLD", "Apply MT above threshold"),
        new DataSourceItem("COMPARE_AND_BEST_MATCH", "Compare and best match")
    };
}