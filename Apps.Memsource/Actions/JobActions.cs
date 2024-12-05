using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Mime;
using Apps.PhraseTMS.Models;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class JobActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public JobActions(IFileManagementClient fileManagementClient)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List jobs", Description = "List all jobs in the project")]
    public async Task<
        ListAllJobsResponse> ListAllJobs(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest input,
        [ActionParameter] ListAllJobsQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = $"/api2/v2/projects/{input.ProjectUId}/jobs";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get,
            authenticationCredentialsProviders);

        var response = await client.Paginate<JobDto>(request);
        response.ForEach(x => x.Project = new()
        {
            UId = input.ProjectUId
        });

        return new()
        {
            Jobs = response
        };
    }

    [Action("Get job", Description = "Get job by UId")]
    public async Task<JobResponse> GetJob(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}",
            Method.Get, authenticationCredentialsProviders);

        var response = await client.ExecuteWithHandling<JobDto>(request);

        return new()
        {
            Uid = response.Uid,
            Filename = response.Filename,
            TargetLanguage = response.TargetLang,
            Status = response.Status,
            ProjectUid = response.Project.UId,
            WordCount = response.WordsCount,
            SourceLanguage = response.SourceLang,
        };
    }

    [Action("Create job", Description = "Create a new job")]
    public async Task<CreateJobResponse> CreateJob(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] CreateJobRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs",
            Method.Post, authenticationCredentialsProviders);

        var output = JsonConvert.SerializeObject(new
        {
            targetLangs = input.TargetLanguages
        });

        var headers = new Dictionary<string, string>()
        {
            { "Memsource", output },
            { "Content-Disposition", $"filename*=UTF-8''{input.File.Name}" },
            { "Content-Type", "application/octet-stream" },
        };
        headers.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

        var fileBytes = _fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

        var response = await client.ExecuteWithHandling<JobResponseWrapper>(request);

        return response.Jobs.First();
    }

    [Action("Delete job", Description = "Delete job by id")]
    public Task DeleteJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] DeleteJobRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/batch",
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
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] GetSegmentsRequest input,
        [ActionParameter] GetSegmentsQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/segments";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query),
            Method.Get, authenticationCredentialsProviders);

        return await client.ExecuteWithHandling<GetSegmentsResponse>(request);
    }

    [Action("Edit job", Description = "Edit selected job")]
    public async Task<JobResponse> EditJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input,
        [ActionParameter] EditJobBody body,
        [ActionParameter] [DataSource(typeof(VendorDataHandler))] IEnumerable<string>? Vendors,
        [ActionParameter] [DataSource(typeof(LinguistDataHandler))] IEnumerable<string>? Linguists)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var bodyDictionary = new Dictionary<string, object>
        {
            { "jobs", JsonConvert.SerializeObject(new[]
                {
                    new
                    {
                        uid = input.JobUId
                    }
                })
            }
        };

        if (body.Status != null)
        {
            bodyDictionary.Add("status", body.Status);
        }

        if (body.DateDue.HasValue)
        {
            bodyDictionary.Add("dateDue", body.DateDue);
        }
        if (Vendors != null && Vendors.Any())
        {

            bodyDictionary.Add("providers", JsonConvert.SerializeObject(Vendors.Select(x =>
                    new
                    {
                        type = "VENDOR",
                        id = x
                    }).ToArray()));
        }

        if (Linguists != null && Linguists.Any())
        {
            bodyDictionary.Add("providers", JsonConvert.SerializeObject(Linguists.Select(x =>
                    new
                    {
                        type = "USER",
                        id = x
                    }).ToArray()));
        }

        var request = new PhraseTmsRequest($"/api2/v3/jobs",
            Method.Patch, authenticationCredentialsProviders)
            .WithJsonBody(bodyDictionary, JsonConfig.DateSettings);


        var response = await client.ExecuteWithHandling<JobDto>(request);
        return new()
        {
            Uid = response.Uid,
            Filename = response.Filename,
            TargetLanguage = response.TargetLang,
            Status = response.Status,
            ProjectUid = response.Project.UId,
            WordCount = response.WordsCount,
            SourceLanguage = response.SourceLang,
        };
    }


    [Action("Download target file", Description = "Download target file of a job")]
    public async Task<TargetFileResponse> DownloadTargetFile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var requestFile = new PhraseTmsRequest(
            $"/api2/v2/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/targetFile",
            Method.Put, authenticationCredentialsProviders);
        var asyncRequest = await client.PerformAsyncRequest(requestFile, authenticationCredentialsProviders);

        if (asyncRequest is null) throw new("Failed creating asynchronous target file request");

        var requestDownload = new PhraseTmsRequest(
            $"/api2/v2/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/downloadTargetFile/{asyncRequest.Id}?format={"ORIGINAL"}",
            Method.Get, authenticationCredentialsProviders);
        var responseDownload = await client.ExecuteWithHandling(requestDownload);

        if (responseDownload == null) throw new("Failed downloading target files");

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = Uri.UnescapeDataString(filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1]);
        string mimeType = "";
        if (MimeTypes.TryGetMimeType(filename, out mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        using var stream = new MemoryStream(fileData);
        var file = await _fileManagementClient.UploadAsync(stream, mimeType, filename);
        return new() { File = file };
    }

    [Action("Download original file", Description = "Download original file of a job")]
    public async Task<TargetFileResponse> DownloadOriginalFile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var requestFile = new PhraseTmsRequest(
            $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/original?format=ORIGINAL",
            Method.Get, authenticationCredentialsProviders);

        var responseDownload = await client.ExecuteWithHandling(requestFile);

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = Uri.UnescapeDataString(filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1]);

        using var stream = new MemoryStream(fileData);
        var file = await _fileManagementClient.UploadAsync(stream, responseDownload.ContentType, filename);
        return new() { File = file };
    }

    [Action("Update target file", Description = "Update target file of a job")]
    public Task UpdateTargetFile(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest job,
        [ActionParameter] UpdateTargetFileInput input)
    {
        var client = new PhraseTmsClient(creds);

        var jsonPayload = JsonConvert.SerializeObject(new UpdateTargetFileRequest()
            {
                Jobs = new[]
                {
                    new UidRequest()
                    {
                        Uid = job.JobUId
                    }
                },
                PropagateConfirmedToTm = input.PropagateConfirmedToTm ?? default,
                UnconfirmChangedSegments = input.UnconfirmChangedSegments ?? default
            }, JsonConfig.Settings)
            .Replace("\r", string.Empty)
            .Replace("\n", string.Empty)
            .Replace(" ", string.Empty);

        var fileBytes = _fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        var request = new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/target", Method.Post, creds)
            .AddHeader("Content-Disposition", $"filename*=UTF-8''{input.FileName ?? input.File.Name}")
            .AddHeader("Content-Type", "application/octet-stream")
            .AddHeader("Memsource", jsonPayload)
            .AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

        return client.ExecuteWithHandling(request);
    }

    [Action("Download bilingual file", Description = "Download bilingual file for a job")]
    public async Task<TargetFileResponse> DownloadBilingualFile(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input,
        [ActionParameter] BilingualRequest bilingualRequest)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var requestFile = new PhraseTmsRequest(
            $"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/bilingualFile",
            Method.Post, authenticationCredentialsProviders);
        if (!String.IsNullOrEmpty(bilingualRequest.Format)) 
        { requestFile.AddQueryParameter("format",bilingualRequest.Format); }
        if (bilingualRequest.Preview != null && bilingualRequest.Preview is false)
        { requestFile.AddQueryParameter("preview", "false"); }
        var jsonBody = JsonConvert.SerializeObject(new
        {
            jobs = new[]
                {
                    new 
                    {
                        uid = input.JobUId
                    }
                },
        });

        requestFile.AddJsonBody(jsonBody);

        var responseDownload = await client.ExecuteWithHandling(requestFile);

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = Uri.UnescapeDataString(filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1]);

        using var stream = new MemoryStream(fileData);
        var file = await _fileManagementClient.UploadAsync(stream, responseDownload.ContentType, filename);
        return new() { File = file };
    }

    [Action("Upload bilingual file", Description = "Upload bilingual file to update job")]
    public Task UploadBilingualFile(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] UploadBilingualFileRequest input)
    {
        var client = new PhraseTmsClient(creds);

        var fileBytes = _fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        var request = new PhraseTmsRequest($"/api2/v2/bilingualFiles", Method.Post, creds);
        if (!String.IsNullOrEmpty(input.saveToTransMemory))
        { request.AddQueryParameter("saveToTransMemory", input.saveToTransMemory); }
        if (input.setCompleted != null && input.setCompleted is true)
        { request.AddQueryParameter("setCompleted", true); }
        request.AlwaysMultipartFormData = true;
        request.AddFile("file", fileBytes, input.File.Name);

        return client.ExecuteWithHandling(request);
    }
}