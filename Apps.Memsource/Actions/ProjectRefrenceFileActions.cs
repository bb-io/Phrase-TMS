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
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.PhraseTMS.Actions;

[ActionList("Reference files")]
public class ProjectRefrenceFileActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Search project reference files", Description = "Searches through all project reference files")]
    public async Task<ListReferenceFilesResponse> ListReferenceFiles( [ActionParameter] ProjectRequest input, [ActionParameter] ListReferenceFilesQuery query)
    {
        var endpoint = $"/api2/v1/projects/{input.ProjectUId}/references";
        var request = new RestRequest(endpoint.WithQuery(query), Method.Get);

        var response = await Client.Paginate<ReferenceFileInfoDto>(request);

        return new()
        {
            ReferenceFileInfo = response
        };
    }

    [Action("Add project reference files", Description = "Add new project reference files. In case no file parts are sent, only 1 reference is created with the given note. Either at least one file must be sent or the note must be specified.")]
    public async Task<ListReferenceFilesResponse> CreateReferenceFile([ActionParameter] ProjectRequest projectRequest, [ActionParameter] CreateReferenceFileRequest input)
    {
        if (input.Files == null && string.IsNullOrEmpty(input.Note))
        {
            throw new PluginMisconfigurationException("At least one of the inputs (Reference files or note) must be specified");
        }
        
        RestRequest request = new RestRequest($"/api2/v2/projects/{projectRequest.ProjectUId}/references", Method.Post);
        request.AlwaysMultipartFormData = true;
        
        if (input.Files != null)
        {
            foreach (var file in input.Files)
            {
                var fileStream = await fileManagementClient.DownloadAsync(file);
                var bytes = await fileStream.GetByteData();
                request = request.AddFile("file", bytes, file.Name, file.ContentType);
            }
        }

        if (!string.IsNullOrEmpty(input.Note))
        {
            var noteJson = JsonConvert.SerializeObject(new { note = input.Note });
            request = request.AddParameter("json", noteJson, ParameterType.RequestBody);
        }

        var response = await Client.ExecuteWithHandling<CreateReferenceFilesDto>(request);
        return new()
        {
            ReferenceFileInfo = response.ReferenceFiles
        };
    }

    [Action("Download project reference file", Description = "Download project reference file")]
    public async Task<DownloadReferenceFilesResponse> DownloadReferenceFiles([ActionParameter] ReferenceFileRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{input.ProjectUId}/references/{input.ReferenceFileUId}", Method.Get);
        var response = await Client.ExecuteWithHandling(request);

        byte[] fileData = response.RawBytes;
        var filenameHeader = response.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1];
        string mimeType = "";
        if (MimeTypes.TryGetMimeType(filename, out mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        using var stream = new MemoryStream(response.RawBytes);
        var file = await fileManagementClient.UploadAsync(stream, mimeType, filename);
        return new() { File = file };
    }

    [Action("Delete project reference file", Description = "Delete specific project reference file")]
    public Task DeleteReferenceFile([ActionParameter] ReferenceFileRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{input.ProjectUId}/references", Method.Delete);
        request.WithJsonBody(new
        {
            referenceFiles = new[] { new { id = input.ReferenceFileUId } }
        });

        return Client.ExecuteWithHandling(request);
    }
}