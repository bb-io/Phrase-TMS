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
using File = Blackbird.Applications.Sdk.Common.Files.File;
using System.Net.Mime;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class FileActions
    {
        [Action("List all files", Description = "List all files")]
        public async Task<ListAllFilesResponse> ListAllFiles(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListFilesQuery query)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);

            var endpoint = "/api2/v1/files";
            var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

            var response = await client.Paginate<FileInfoDto>(request);

            return new ListAllFilesResponse
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

            var response = (await client.ExecuteAsync(request)).RawBytes;

            return new GetFileResponse
            {
                File = new File(response) 
                { 
                    Name = input.FileUId,
                    ContentType = MediaTypeNames.Application.Octet
                }
            };
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
            request.AddParameter("application/octet-stream", input.File.Bytes, ParameterType.RequestBody);
            request.WithJsonBody(input,  JsonConfig.Settings);
            
            return client.ExecuteWithHandling<FileInfoDto>(request);
        }
    }
}