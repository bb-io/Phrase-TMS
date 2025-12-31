using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common.Invocation;
using System.Reflection;

namespace PhraseTMSTests.Base;

[AttributeUsage(AttributeTargets.Method)]
public class ContextDataSourceAttribute(params string[] connectionTypeFilters) : Attribute, ITestDataSource
{
    public IEnumerable<object[]> GetData(MethodInfo methodInfo)
    {
        var testBase = new TestBase();
        var allContexts = testBase.InvocationContexts;

        IEnumerable<InvocationContext> contextsToRun;

        if (connectionTypeFilters.Length == 0)
            contextsToRun = allContexts;
        else
        {
            contextsToRun = allContexts.Where(ctx =>
            {
                var type = ctx.AuthenticationCredentialsProviders.FirstOrDefault(p => p.KeyName == CredsNames.ConnectionType)?.Value;
                return type != null && connectionTypeFilters.Contains(type);
            }).ToList();

            if (!contextsToRun.Any())
            {
                var specifiedTypes = string.Join("', '", connectionTypeFilters);
                throw new ArgumentException($"Connection types were not found: '{specifiedTypes}'");
            }
        }

        return contextsToRun.Select(ctx => new object[] { ctx });
    }

    public string? GetDisplayName(MethodInfo methodInfo, object?[]? data)
    {
        if (data == null) return null;
        var context = data[0] as InvocationContext ?? throw new ArgumentNullException(nameof(data));

        var connectionValue = context.AuthenticationCredentialsProviders
            .First(p => p.KeyName == CredsNames.ConnectionType)
            .Value;

        return $"Connection type: {connectionValue}";
    }
}
