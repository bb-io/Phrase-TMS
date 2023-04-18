using Apps.PhraseTms.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ListAllFilesResponse ListAllFiles(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/files", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<ResponseWrapper<List<FileInfoDto>>>(request);
            return new ListAllFilesResponse()
            {
                Files = response.Content
            };
        }

        [Action("Get file", Description = "Get file")]
        public GetFileResponse GetFile(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetFileRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/files/{input.FileUId}", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<string>(request);
            return new GetFileResponse()
            {
                FileContent = response
            };
        }

        [Action("Upload file", Description = "Upload file")]
        public FileInfoDto UploadFile(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] UploadFileRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/files", Method.Post, authenticationCredentialsProvider.Value);
            request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.FileName}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);
            var response = client.Execute<FileInfoDto>(request).Data;
            return response;
        }
    }
}
