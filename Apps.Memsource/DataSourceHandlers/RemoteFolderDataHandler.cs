using Apps.PhraseTMS.Dtos.Connectors;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.SDK.Extensions.FileManagement.Models.FileDataSourceItems;
using RestSharp;

namespace Apps.PhraseTMS.DataSourceHandlers;

public class RemoteFolderDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] CreateJobsFromRemoteFileRequest input)
    : PhraseInvocable(invocationContext), IAsyncFileDataSourceItemHandler
{
    private const string RootFolderId = "/";

    public async Task<IEnumerable<FileDataItem>> GetFolderContentAsync(
        FolderContentDataSourceContext context,
        CancellationToken cancellationToken)
    {
        var connector = await GetConnector(input.ConnectorToken);
        var folderId = string.IsNullOrWhiteSpace(context?.FolderId) ? RootFolderId : context.FolderId;
        var endpoint = folderId == RootFolderId
            ? $"/api2/v1/connectors/{connector.Id}/folders"
            : $"/api2/v1/connectors/{connector.Id}/folders/{folderId}";
        var request = new RestRequest(endpoint, Method.Get)
            .AddQueryParameter("fileType", "FOLDERS_ONLY");
        var response = await Client.ExecuteWithHandling<ConnectorFilesResponse>(request);

        return response.Files
            .Where(x => x.IsDirectory && !string.IsNullOrWhiteSpace(x.EncodedName))
            .OrderBy(x => x.Name)
            .Select(x => (FileDataItem)new Folder
            {
                Id = x.EncodedName,
                DisplayName = x.Name,
                IsSelectable = true
            });
    }

    public async Task<IEnumerable<FolderPathItem>> GetFolderPathAsync(
        FolderPathDataSourceContext context,
        CancellationToken cancellationToken)
    {
        var path = new List<FolderPathItem>
        {
            new() { Id = RootFolderId, DisplayName = "/" }
        };

        if (string.IsNullOrWhiteSpace(context?.FileDataItemId)
            || context.FileDataItemId == RootFolderId)
        {
            return path;
        }

        var connector = await GetConnector(input.ConnectorToken);
        var request = new RestRequest(
            $"/api2/v1/connectors/{connector.Id}/folders/{context.FileDataItemId}",
            Method.Get)
            .AddQueryParameter("fileType", "FOLDERS_ONLY");
        var response = await Client.ExecuteWithHandling<ConnectorFilesResponse>(request);

        path.Add(new FolderPathItem
        {
            Id = response.EncodedCurrentFolder ?? context.FileDataItemId,
            DisplayName = string.IsNullOrWhiteSpace(response.CurrentFolder)
                ? "Selected folder"
                : response.CurrentFolder
        });

        return path;
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
