﻿using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.Vendors.Response;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;
using Apps.PhraseTMS.Models.ProjectReferenceFiles.Responses;
using Apps.PhraseTms.Models.Jobs.Requests;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class ProjectRefrenceFileActions
    {
        [Action("List all reference files", Description = "List all project reference files")]
        public ListReferenceFilesResponse ListReferenceFiles(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            ListReferenceFilesRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/references", Method.Get, authenticationCredentialsProvider.Value);
            return new ListReferenceFilesResponse()
            {
                ReferenceFileInfo = client.Get<ResponseWrapper<List<ReferenceFileInfoDto>>>(request).Content
            };
        }

        [Action("Create project reference file", Description = "Create project reference file")]
        public void CreateReferenceFile(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateReferenceFileRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/references",
                Method.Post, authenticationCredentialsProvider.Value);

            request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.Filename}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);
            client.Execute(request);
        }

        [Action("Download reference files", Description = "Download project reference files")]
        public DownloadReferenceFilesResponse DownloadReferenceFiles(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] DownloadReferenceFilesRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/references/{input.ReferenceFileUId}", 
                Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Execute(request);

            byte[] fileData = response.RawBytes;
            var filenameHeader = response.ContentHeaders.First(h => h.Name == "Content-Disposition");
            var filename = filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1];
            return new DownloadReferenceFilesResponse()
            {
                File = fileData,
                Filename = filename
            };
        }

        [Action("Delete reference file", Description = "Delete reference file")]
        public void DeleteReferenceFile(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] DeleteReferenceFileRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/references",
                Method.Delete, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                referenceFiles = new[] { new { id = input.ReferenceFileUId } }
            });
            client.Execute(request);
        }
    }
}