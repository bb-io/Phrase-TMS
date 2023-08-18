using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTms.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class JobActions
    {
        [Action("List jobs", Description = "List all jobs in the project")]
        public async Task<
            ListAllJobsResponse> ListAllJobs(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListAllJobsPathRequest input,
            [ActionParameter] ListAllJobsQuery query)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);

            var endpoint = $"/api2/v2/projects/{input.ProjectUId}/jobs";
            var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get,
                authenticationCredentialsProviders);

            var response = await client.Paginate<JobDto>(request);

            return new ListAllJobsResponse
            {
                Jobs = response
            };
        }

        [Action("Get job", Description = "Get job by UId")]
        public async Task<JobResponse> GetJob(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetJobRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}",
                Method.Get, authenticationCredentialsProviders);

            var response = await client.ExecuteWithHandling<JobDto>(request);

            return new JobResponse
            {
                Uid = response.Uid,
                Filename = response.Filename,
                TargetLanguage = response.TargetLang,
                Status = response.Status,
                ProjectUid = response.Project.UId,
                //DateDue = response.DateDue
            };
        }

        [Action("Create job", Description = "Create a new job")]
        public async Task<JobDto> CreateJob(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateJobRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs",
                Method.Post, authenticationCredentialsProviders);

            var output = JsonConvert.SerializeObject(new
            {
                targetLangs = input.TargetLanguages
            });
            
            var headers = new Dictionary<string, string>()
            {
                { "Memsource", output },
                { "Content-Disposition", $"filename*=UTF-8''{input.FileName}.{input.FileType}" },
                { "Content-Type", "application/octet-stream" },
            };

            headers.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);

            var response = await client.ExecuteWithHandling<JobResponseWrapper>(request);

            return response.Jobs.First();
        }

        [Action("Delete job", Description = "Delete job by id")]
        public Task DeleteJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteJobRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/batch",
                Method.Delete, authenticationCredentialsProviders);
            request.WithJsonBody(new
            {
                jobs = input.JobsUIds.Select(u => new { uid = u })
            });

            return client.ExecuteWithHandling(request);
        }

        [Action("Get segments", Description = "Get all segments in job")]
        public async Task<GetSegmentsResponse> GetSegments(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetSegmentsRequest input,
            [ActionParameter] GetSegmentsQuery query)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);

            var endpoint = $"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/segments";
            var request = new PhraseTmsRequest(endpoint.WithQuery(query),
                Method.Get, authenticationCredentialsProviders);

            var response = await client.ExecuteWithHandling(request);

            dynamic content = (JObject)JsonConvert.DeserializeObject(response.Content);
            JArray segmentsArr = content.segments;
            var segments = segmentsArr.ToObject<List<SegmentDto>>();
            return new GetSegmentsResponse
            {
                Segments = segments
            };
        }

        [Action("Edit job", Description = "Edit selected job")]
        public Task EditJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] EditJobPath input,
            [ActionParameter] EditJobBody body)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}",
                Method.Patch, authenticationCredentialsProviders);
            request.WithJsonBody(body,  JsonConfig.Settings);

            return client.ExecuteWithHandling(request);
        }

        [Action("Download target file", Description = "Download target file of a job")]
        public async Task<TargetFileResponse> DownloadTargetFile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TargetFileRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var requestFile = new PhraseTmsRequest(
                $"/api2/v2/projects/{input.ProjectUId}/jobs/{input.JobUId}/targetFile",
                Method.Put, authenticationCredentialsProviders);
            var asyncRequest = client.PerformAsyncRequest(requestFile, authenticationCredentialsProviders);

            if (asyncRequest is null) throw new Exception("Failed creating asynchronous target file request");

            var requestDownload = new PhraseTmsRequest(
                $"/api2/v2/projects/{input.ProjectUId}/jobs/{input.JobUId}/downloadTargetFile/{asyncRequest.Id}?format={"ORIGINAL"}",
                Method.Get, authenticationCredentialsProviders);
            var responseDownload = await client.ExecuteWithHandling(requestDownload);

            if (responseDownload == null) throw new Exception("Failed downloading target files");

            var fileData = responseDownload.RawBytes;
            var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
            var filename = filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1];

            return new TargetFileResponse
            {
                Filename = filename,
                File = fileData
            };
        }
    }
}