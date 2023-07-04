using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.Files.Responses;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Files.Requests;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class FileActions
    {
        [Action("List all files", Description = "List all files")]
        public async Task<ListAllFilesResponse> ListAllFiles(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/files", Method.Get, authenticationCredentialsProviders);

            var response = await client.ExecuteWithHandling(()
                => client.ExecuteGetAsync<ResponseWrapper<List<FileInfoDto>>>(request));

            return new ListAllFilesResponse
            {
                Files = response.Content
            };
        }

        [Action("Get file", Description = "Get file")]
        public async Task<GetFileResponse> GetFile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetFileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/files/{input.FileUId}", Method.Get,
                authenticationCredentialsProviders);

            var response = await client.ExecuteWithHandling(() => client.ExecuteGetAsync<string>(request));

            return new GetFileResponse
            {
                FileContent = response
            };
        }

        [Action("Upload file", Description = "Upload file")]
        public Task<FileInfoDto> UploadFile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] UploadFileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/files", Method.Post, authenticationCredentialsProviders);
            request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.FileName}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);
            
            return client.ExecuteWithHandling(() => client.ExecuteAsync<FileInfoDto>(request));
        }
    }
}