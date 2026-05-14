using Apps.PhraseTMS.Dtos.AutomatedSettings;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class AutomatedProjectSettingsDataHandler(InvocationContext context) 
    : PhraseInvocable(context), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken ct)
    {
        var request = new RestRequest("api2/v1/automatedProjects");
        var response = await Client.PaginateOnce<AutomatedProjectSettingDto>(request);
        return response.Where(x =>
            string.IsNullOrEmpty(context.SearchString) ||
            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Id, x.Name))
            .ToList();
    }
}