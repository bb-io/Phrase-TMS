using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Extension;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Responses;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class ProjectRefrenceFileActions
    {
        [Action("List all reference files", Description = "List all project reference files")]
        public async Task<ListReferenceFilesResponse> ListReferenceFiles(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListReferenceFilesRequest input,
            [ActionParameter] ListReferenceFilesQuery query)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);

            var endpoint = $"/api2/v1/projects/{input.ProjectUId}/references";
            var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get,
                authenticationCredentialsProviders);

            var response = await client.ExecuteWithHandling<ResponseWrapper<List<ReferenceFileInfoDto>>>(request);

            return new ListReferenceFilesResponse
            {
                ReferenceFileInfo = response.Content
            };
        }

        [Action("Create project reference file", Description = "Create project reference file")]
        public Task<ReferenceFileInfoDto> CreateReferenceFile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateReferenceFileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/references",
                Method.Post, authenticationCredentialsProviders);

            request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.FileName}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);

            return client.ExecuteWithHandling<ReferenceFileInfoDto>(request);
        }

        [Action("Download reference files", Description = "Download project reference files")]
        public async Task<DownloadReferenceFilesResponse> DownloadReferenceFiles(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DownloadReferenceFilesRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest(
                $"/api2/v1/projects/{input.ProjectUId}/references/{input.ReferenceFileUId}",
                Method.Get, authenticationCredentialsProviders);
            var response = await client.ExecuteWithHandling(request);

            byte[] fileData = response.RawBytes;
            var filenameHeader = response.ContentHeaders.First(h => h.Name == "Content-Disposition");
            var filename = filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1];
            return new DownloadReferenceFilesResponse
            {
                File = fileData,
                Filename = filename
            };
        }

        [Action("Delete reference file", Description = "Delete reference file")]
        public Task DeleteReferenceFile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteReferenceFileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/references",
                Method.Delete, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                referenceFiles = new[] { new { id = input.ReferenceFileUId } }
            });

            return client.ExecuteWithHandling(request);
        }
    }
}