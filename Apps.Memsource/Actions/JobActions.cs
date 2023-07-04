﻿using Apps.PhraseTms.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using Apps.PhraseTms.Dtos;
using Apps.PhraseTms.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTms.Actions
{
    [ActionList]
    public class JobActions
    {
        [Action("List all jobs", Description = "List all jobs in the project")]
        public ListAllJobsResponse ListAllJobs(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListAllJobsRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v2/projects/{input.ProjectUId}/jobs", Method.Get, authenticationCredentialsProviders);
            var response = client.Get<ResponseWrapper<List<JobDto>>>(request);
            return new ListAllJobsResponse()
            {
                Jobs = response.Content
            };
        }

        [Action("Get job", Description = "Get job by UId")]
        public GetJobResponse GetJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetJobRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}", 
                Method.Get, authenticationCredentialsProviders);
            var response = client.Get<JobDto>(request);

            return new GetJobResponse()
            {
                Filename = response.Filename,
                TargetLanguage = response.TargetLang,
                Status = response.Status,
                DateDue = response.DateDue
            };
        }

        [Action("Create job", Description = "Create job")]
        public JobDto CreateJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateJobRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs", 
                Method.Post, authenticationCredentialsProviders);

            string output = JsonConvert.SerializeObject(new
            {
                targetLangs = input.TargetLanguages
            });

            request.AddHeader("Memsource", output);
            request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.FileName}.{input.FileType}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);
            return client.Execute<JobResponseWrapper>(request).Data.Jobs.First();
        }

        [Action("Delete job", Description = "Delete job by id")]
        public void DeleteJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteJobRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/batch", 
                Method.Delete, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                jobs = input.JobsUIds.Select(u => new { uid = u })
            });
            client.Execute(request);
        }

        [Action("Get segments", Description = "Get segments in job")]
        public GetSegmentsResponse GetSegments(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetSegmentsRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/segments?beginIndex={input.BeginIndex}&endIndex={input.EndIndex}", 
                Method.Get, authenticationCredentialsProviders);
            var response = client.Get(request);
            dynamic content = (JObject)JsonConvert.DeserializeObject(response.Content);
            JArray segmentsArr = content.segments;
            var segments = segmentsArr.ToObject<List<SegmentDto>>();
            return new GetSegmentsResponse()
            {
                Segments = segments
            };
        }

        [Action("Edit job", Description = "Edit job")]
        public void EditJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] EditJobRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}", 
                Method.Patch, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                dateDue = input.DateDue,
                status = input.Status,
            });
            client.Execute(request);
        }

        [Action("Download target file", Description = "Download target file")]
        public TargetFileResponse DownloadTargetFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TargetFileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var requestFile = new PhraseTmsRequest($"/api2/v2/projects/{input.ProjectUId}/jobs/{input.JobUId}/targetFile", 
                Method.Put, authenticationCredentialsProviders);
            var asyncRequest = client.PerformAsyncRequest(requestFile, authenticationCredentialsProviders);

            if (asyncRequest is null) throw new Exception("Failed creating asynchronous target file request");

            var requestDownload = new PhraseTmsRequest($"/api2/v2/projects/{input.ProjectUId}/jobs/{input.JobUId}/downloadTargetFile/{asyncRequest.Id}?format={"ORIGINAL"}",
                Method.Get, authenticationCredentialsProviders);
            var responseDownload = client.Get(requestDownload);

            if (responseDownload == null) throw new Exception("Failed downloading target files");
            
            byte[] fileData = responseDownload.RawBytes;
            var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
            var filename = filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1];

            return new TargetFileResponse()
            {
                Filename = filename,
                File = fileData
            };
        }
    }
}
