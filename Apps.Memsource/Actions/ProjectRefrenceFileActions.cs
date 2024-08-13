using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using System.Net.Mime;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class ProjectRefrenceFileActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public ProjectRefrenceFileActions(IFileManagementClient fileManagementClient)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List reference files", Description = "List all project reference files")]
    public async Task<ListReferenceFilesResponse> ListReferenceFiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest input,
        [ActionParameter] ListReferenceFilesQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = $"/api2/v1/projects/{input.ProjectUId}/references";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get,
            authenticationCredentialsProviders);

        var response = await client.Paginate<ReferenceFileInfoDto>(request);

        return new()
        {
            ReferenceFileInfo = response
        };
    }

    [Action("Create reference files", Description = "Create a new project reference files. In case no file parts are sent, only 1 reference is created with the given note. Either at least one file must be sent or the note must be specified.")]
    public async Task<ListReferenceFilesResponse> CreateReferenceFile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] CreateReferenceFileRequest input)
    {
        if (input.Files == null && string.IsNullOrEmpty(input.Note))
        {
            throw new ArgumentException("At least one of the inputs (Reference files or note) must be specified");
        }
        
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        RestRequest request = new PhraseTmsRequest($"/api2/v2/projects/{projectRequest.ProjectUId}/references",
            Method.Post, authenticationCredentialsProviders);
        request.AlwaysMultipartFormData = true;
        
        if (input.Files != null)
        {
            foreach (var file in input.Files)
            {
                var fileStream = await _fileManagementClient.DownloadAsync(file);
                var bytes = await fileStream.GetByteData();
                request = request.AddFile("file", bytes, file.Name, file.ContentType);
            }
        }

        if (!string.IsNullOrEmpty(input.Note))
        {
            var noteJson = JsonConvert.SerializeObject(new { note = input.Note });
            request = request.AddParameter("json", noteJson, ParameterType.RequestBody);
        }

        var response = await client.ExecuteWithHandling<CreateReferenceFilesDto>(request);
        return new()
        {
            ReferenceFileInfo = response.ReferenceFiles
        };
    }

    [Action("Download reference file", Description = "Download project reference file")]
    public async Task<DownloadReferenceFilesResponse> DownloadReferenceFiles(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ReferenceFileRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest(
            $"/api2/v1/projects/{input.ProjectUId}/references/{input.ReferenceFileUId}",
            Method.Get, authenticationCredentialsProviders);
        var response = await client.ExecuteWithHandling(request);

        byte[] fileData = response.RawBytes;
        var filenameHeader = response.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1];
        string mimeType = "";
        if (MimeTypes.TryGetMimeType(filename, out mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        using var stream = new MemoryStream(response.RawBytes);
        var file = await _fileManagementClient.UploadAsync(stream, mimeType, filename);
        return new() { File = file };
    }

    [Action("Delete reference file", Description = "Delete specific project reference file")]
    public Task DeleteReferenceFile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ReferenceFileRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/references",
            Method.Delete, authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            referenceFiles = new[] { new { id = input.ReferenceFileUId } }
        });

        return client.ExecuteWithHandling(request);
    }
}