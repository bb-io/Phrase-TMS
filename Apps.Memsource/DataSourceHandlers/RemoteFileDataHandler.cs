using Apps.PhraseTMS.Dtos.Connectors;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class RemoteFileDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] CreateJobsFromRemoteFileRequest input)
    : PhraseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(
        DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(input.RemoteFolder))
        {
            throw new PluginMisconfigurationException("Please select a remote folder first");
        }

        var connector = await GetConnector(input.ConnectorToken);
        var endpoint = input.RemoteFolder == "/"
            ? $"/api2/v1/connectors/{connector.Id}/folders"
            : $"/api2/v1/connectors/{connector.Id}/folders/{input.RemoteFolder}";
        var request = new RestRequest(endpoint, Method.Get)
            .AddQueryParameter("fileType", "FILES_ONLY");
        var response = await Client.ExecuteWithHandling<ConnectorFilesResponse>(request);

        var files = response.Files
            .Where(x => !x.IsDirectory && !string.IsNullOrWhiteSpace(x.EncodedName))
            .Where(x => string.IsNullOrWhiteSpace(context.SearchString)
                || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToArray();
        var duplicateNames = files
            .GroupBy(x => x.Name, StringComparer.Ordinal)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToHashSet(StringComparer.Ordinal);

        return files
            .OrderBy(x => x.Name)
            .Select(x => new DataSourceItem(
                x.EncodedName,
                GetDisplayName(x, duplicateNames.Contains(x.Name))));
    }

    private static string GetDisplayName(ConnectorFileDto file, bool hasDuplicateName)
    {
        if (!hasDuplicateName)
        {
            return file.Name;
        }

        var identifier = file.Id;
        try
        {
            identifier = JObject.Parse(file.Id).Value<string>("entryId") ?? file.Id;
        }
        catch
        {
            // Connector IDs are provider-specific; use the raw ID when it is not JSON.
        }

        return string.IsNullOrWhiteSpace(identifier)
            ? file.Name
            : $"{file.Name} ({identifier})";
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
