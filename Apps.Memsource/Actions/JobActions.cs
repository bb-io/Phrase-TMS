using Apps.Memsource.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using Apps.Memsource.Dtos;
using Apps.Memsource.Models.Jobs.Responses;
using System.Net.Http;
using System.Net.Mime;
using Apps.Memsource.Models.Projects.Requests;

namespace Apps.Memsource.Actions
{
    [ActionList]
    public class JobActions
    {
        [Action("Get job", Description = "Get job by UId")]
        public GetJobResponse GetJob(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetJobRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}", 
                Method.Get, "ApiToken " + authenticationCredentialsProvider.Value);
            var response = client.Get(request);

            var job = JsonConvert.DeserializeObject<JobDto>(response.Content);

            return new GetJobResponse()
            {
                Filename = job.Filename,
                TargetLanguage = job.TargetLang,
                Status = job.Status,
                DateDue = job.DateDue
            };
        }

        [Action("Create job", Description = "Create job")]
        public void CreateJob(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateJobRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}/jobs", 
                Method.Post, "ApiToken " + authenticationCredentialsProvider.Value);

            string output = JsonConvert.SerializeObject(new
            {
                targetLangs = input.TargetLanguages
            });

            request.AddHeader("Memsource", output);
            request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.FileName}.{input.FileType}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);
            client.Execute(request);
        }

        [Action("Delete job", Description = "Delete job by id")]
        public void DeleteJob(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] DeleteJobRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/batch", 
                Method.Delete, "ApiToken " + authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                jobs = input.JobsUIds.Select(u => new { uid = u })
            });
            client.Execute(request);
        }

        [Action("Get segments", Description = "Get segments in job")]
        public GetSegmentsResponse GetSegments(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetSegmentsRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}/segments?beginIndex={input.BeginIndex}&endIndex={input.EndIndex}", 
                Method.Get, "ApiToken " + authenticationCredentialsProvider.Value);
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
        public void EditJob(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] EditJobRequest input)
        {
            var client = new MemsourceClient(url);
            var request = new MemsourceRequest($"/api2/v1/projects/{input.ProjectUId}/jobs/{input.JobUId}", 
                Method.Patch, "ApiToken " + authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                dateDue = input.DateDue,
                status = input.Status,
            });
            client.Execute(request);
        }

        [Action("Download target file", Description = "Download target file")]
        public TargetFileResponse DownloadTargetFile(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TargetFileRequest input)
        {
            var client = new MemsourceClient(url);
            var requestFile = new MemsourceRequest($"/api2/v2/projects/{input.ProjectUId}/jobs/{input.JobUId}/targetFile", 
                Method.Put, "ApiToken " + authenticationCredentialsProvider.Value);
            var responseFile = client.Execute(requestFile);
            dynamic contentFile = JsonConvert.DeserializeObject(responseFile.Content);

            Task.Delay(2000);

            var requestDownload = new MemsourceRequest($"/api2/v2/projects/{input.ProjectUId}/jobs/{input.JobUId}/downloadTargetFile/{contentFile.asyncRequest.id}?format={"ORIGINAL"}",
                Method.Get, "ApiToken " + authenticationCredentialsProvider.Value);
            var responseDownload = client.Get(requestDownload);
            byte[] fileData = responseDownload.RawBytes;
            var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
            var filename = filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1];

            Console.WriteLine(filename);
            return new TargetFileResponse()
            {
                Filename = filename,
                File = fileData
            };
        }


        //var requestAsyncStatus = new MemsourceRequest($"/api2/v1/async?all=true",
        //        Method.Get, "ApiToken " + authenticationCredentialsProvider.Value);
        //var responseAsyncStatus = client.Get(requestAsyncStatus);
        //dynamic contentAsyncStatus = JsonConvert.DeserializeObject(responseAsyncStatus.Content);
        //JArray asyncReqArr = contentAsyncStatus.content;
        //var asyncRequests = asyncReqArr.ToObject<List<AsyncRequestDto>>();
        //var isPending = asyncRequests.Any(r => r.Id == (string)contentFile.asyncRequest.id);
        //Console.WriteLine(isPending);
    }
}
