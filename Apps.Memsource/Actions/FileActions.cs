using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Files.Responses;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Files.Requests;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class FileActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public FileActions(IFileManagementClient fileManagementClient)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List all files", Description = "List all files")]
    public async Task<ListAllFilesResponse> ListAllFiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListFilesQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/files";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<FileInfoDto>(request);

        return new()
        {
            Files = response
        };
    }

    [Action("Get file", Description = "Get specific uploaded file")]
    public async Task<GetFileResponse> GetFile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetFileRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/files/{input.FileUId}", Method.Get,
            authenticationCredentialsProviders);

        var response = await client.ExecuteAsync(request);
        using var stream = new MemoryStream(response.RawBytes);
        var file = await _fileManagementClient.UploadAsync(stream, response.ContentType, input.FileUId);
        return new() { File = file };
    }

    [Action("Upload file", Description = "Upload a new file")]
    public Task<FileInfoDto> UploadFile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] UploadFileRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest("/api2/v1/files", Method.Post, authenticationCredentialsProviders);
        request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.FileName ?? input.File.Name}");
        request.AddHeader("Content-Type", "application/octet-stream");

        var fileBytes = _fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);
        request.WithJsonBody(input, JsonConfig.Settings);

        return client.ExecuteWithHandling<FileInfoDto>(request);
    }
}