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
            var request = new MemsourceRequest($"/projects/{input.ProjectUId}/jobs/{input.JobUId}", Method.Get, "ApiToken " + authenticationCredentialsProvider.Value);
            var response = client.Get(request);
            JObject content = (JObject)JsonConvert.DeserializeObject(response.Content);
            var job = content.ToObject<JobDto>();
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
            var request = new MemsourceRequest($"/projects/{input.ProjectUId}/jobs", Method.Post, "ApiToken " + authenticationCredentialsProvider.Value);

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
            var request = new MemsourceRequest($"/projects/{input.ProjectUId}/jobs/batch", Method.Delete, "ApiToken " + authenticationCredentialsProvider.Value);
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
            var request = new MemsourceRequest($"/projects/{input.ProjectUId}/jobs/{input.JobUId}/segments?beginIndex={input.BeginIndex}&endIndex={input.EndIndex}", 
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
    }
}
