using Newtonsoft.Json;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace PhraseTMSTests.Base;

public class TestBaseMultipleConnections : TestBase
{
    public new TestContext TestContext
    {
        get => base.TestContext!;
        set => base.TestContext = value;
    }

    protected void PrintResult(object result)
    {
        TestContext?.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }

    protected void PrintDataHandlerResult(IEnumerable<DataSourceItem> items)
    {
        foreach (var item in items)
            TestContext?.WriteLine($"{item.Value} - {item.DisplayName}");
    }

    protected void PrintDataHandlerResult(Dictionary<string, string> result)
    {
        foreach (var item in result)
            TestContext?.WriteLine($"{item.Value} - {item.Key}");
    }
}
