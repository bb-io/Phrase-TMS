using Apps.PhraseTMS.Dtos.Connectors;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class RemoteFolderDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] CreateJobsFromRemoteFileRequest input)
    : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(
        DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var connector = await GetConnector(input.ConnectorToken);
        var request = new RestRequest($"/api2/v1/connectors/{connector.Id}/folders", Method.Get)
            .AddQueryParameter("fileType", "FOLDERS_ONLY");
        var response = await Client.ExecuteWithHandling<ConnectorFilesResponse>(request);

        var folders = response.Files
            .Where(x => x.IsDirectory && !string.IsNullOrWhiteSpace(x.EncodedName))
            .Where(x => string.IsNullOrWhiteSpace(context.SearchString)
                || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderBy(x => x.Name)
            .Select(x => new DataSourceItem(x.EncodedName, x.Name));

        return [new DataSourceItem("/", "/"), .. folders];
    }

    private async Task<ConnectorDto> GetConnector(string connectorToken)
    {
        if (string.IsNullOrWhiteSpace(connectorToken))
        {
            throw new PluginMisconfigurationException("Please select a connector first");
        }

        var request = new RestRequest("/api2/v1/connectors", Method.Get);
        var response = await Client.ExecuteWithHandling<ConnectorsResponse>(request);
        return response.Connectors.FirstOrDefault(x => x.LocalToken == connectorToken)
            ?? throw new PluginMisconfigurationException("The selected connector could not be found");
    }
}
