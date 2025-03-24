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
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Dtos.Jobs;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class JobActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Search jobs", Description = "Returns a list of jobs in the project based on specified parameters")]
    public async Task<ListAllJobsResponse> ListAllJobs(
        [ActionParameter] ProjectRequest input,
        [ActionParameter] ListAllJobsQuery query,
        [ActionParameter] JobStatusesRequest jobStatusesRequest,
        [ActionParameter] [Display("LQA Score null?")] bool? LQAScorenull)
    {
        var endpoint = $"/api2/v2/projects/{input.ProjectUId}/jobs";
        var request = new RestRequest(endpoint.WithQuery(query), Method.Get);
        try
        {
            var response = await Client.Paginate<ListJobDto>(request);

            if (LQAScorenull.HasValue && LQAScorenull.Value)
            {
                var lqaEndpoint = "/api2/v1/lqa/assessments";
                var lqaRequest = new RestRequest(lqaEndpoint, Method.Post);
                var bodyDictionary = new Dictionary<string, object>
                {
                    { "jobParts", response.Select(u => new { uid = u.Uid }) }
                };
                
                lqaRequest.WithJsonBody(bodyDictionary);
                var result = await Client.ExecuteWithHandling(lqaRequest);
                var data = JsonConvert.DeserializeObject<LQAAssessmentSimpleDto>(result.Content!);
                var jobsWithoutLQA = data?.AssessmentDetails?
                    .Where(a => a.LqaEnabled && a.AssessmentResult == null)
                    .Select(a => a.JobPartUid)?.ToList() ?? Enumerable.Empty<string>();

                return new()
                {
                    Jobs = response.Where(x => jobsWithoutLQA.Contains(x.Uid))?.ToList() ?? new List<ListJobDto>()
                };
            }

            if (jobStatusesRequest.Statuses != null)
            {
                response = response.Where(x => jobStatusesRequest.Statuses.Contains(x.Status)).ToList();
            }

            return new()
            {
                Jobs = response
            };
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Invalid parameters") || e.Message.Contains("The object referenced by the field") ||
                e.Message.Contains("unsupported locale"))
            {
                throw new PluginMisconfigurationException(e.Message + "Make sure that the input values are correct.");
            }
            
            throw new PluginApplicationException(e.Message);
        }
    }

    [Action("Get job", Description = "Get all job information for a specific job")]
    public async Task<JobDto> GetJob(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}", Method.Get);
        return await Client.ExecuteWithHandling<JobDto>(request);
    }

    [Action("Create job", Description = "Create a new job from a file upload")]
    public async Task<CreatedJobDto> CreateJob(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] CreateJobRequest input)
    {

        if (string.IsNullOrWhiteSpace(projectRequest.ProjectUId))
        {
            throw new PluginMisconfigurationException("Project ID is not provided. Please specify a valid Project ID.");
        }

        if (input.File == null)
        {
            throw new PluginMisconfigurationException("File is not provided. Please upload a file.");
        }

        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs", Method.Post);

        var output = JsonConvert.SerializeObject(new
        {
            targetLangs = new List<string> { input.TargetLanguage },
            preTranslate = input.preTranslate ?? false,
            useProjectFileImportSettings = input.useProjectFileImportSettings ?? true,
            due = input.DueDate ?? null,
        });

        var headers = new Dictionary<string, string>()
        {
            { "Memsource", output },
            { "Content-Disposition", $"filename*=UTF-8''{input.File.Name}" },
            { "Content-Type", "application/octet-stream" },
        };
        headers.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

        var fileStream = await fileManagementClient.DownloadAsync(input.File);
        using (var memoryStream = new MemoryStream())
        {
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            if (memoryStream.ReadByte() == -1)
            {
                throw new PluginMisconfigurationException("The provided file is empty. Please check your file input and try again");
            }

            memoryStream.Position = 0;
            var fileBytes = memoryStream.ToArray();
            request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);
        }

        var jobs = await Client.ExecuteWithHandling<JobResponseWrapper>(request);
        return jobs.Jobs.FirstOrDefault();
    }

    [Action("Create jobs", Description = "Create jobs for multiple target languages")]
    public async Task<JobResponseWrapper> CreateJobs(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] CreateJobsRequest input)
    {

        if (string.IsNullOrWhiteSpace(projectRequest.ProjectUId))
        {
            throw new PluginMisconfigurationException("Project ID is not provided. Please specify a valid Project ID.");
        }

        if (input.File == null)
        {
            throw new PluginMisconfigurationException("File is not provided. Please upload a file.");
        }

        if (input.TargetLanguages == null || !input.TargetLanguages.Any())
        {
            var _projectRequest = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}", Method.Get);
            var project = await Client.ExecuteWithHandling<ProjectDto>(_projectRequest);
            input.TargetLanguages = project.TargetLangs;
        }

        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs", Method.Post);

        var output = JsonConvert.SerializeObject(new
        {
            targetLangs = input.TargetLanguages,
            preTranslate = input.preTranslate ?? false,
            useProjectFileImportSettings = input.useProjectFileImportSettings ?? true,
            due = input.DueDate ?? null,
        });

        var headers = new Dictionary<string, string>()
        {
            { "Memsource", output },
            { "Content-Disposition", $"filename*=UTF-8''{input.File.Name}" },
            { "Content-Type", "application/octet-stream" },
        };
        headers.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

        var fileStream = await fileManagementClient.DownloadAsync(input.File);
        using (var memoryStream = new MemoryStream())
        {
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            if (memoryStream.ReadByte() == -1)
            {
                throw new PluginMisconfigurationException("The provided file is empty. Please check your file input and try again");
            }

            memoryStream.Position = 0;
            var fileBytes = memoryStream.ToArray();
            request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);
        }

        return await Client.ExecuteWithHandling<JobResponseWrapper>(request);
    }

    [Action("Delete jobs", Description = "Delete jobs from a project")]
    public Task DeleteJob([ActionParameter] ProjectRequest projectRequest, [ActionParameter] DeleteJobRequest input)
    {
        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/batch", Method.Delete);
        request.WithJsonBody(new
        {
            jobs = input.JobsUIds.Select(u => new { uid = u })
        });

        return Client.ExecuteWithHandling(request);
    }

    [Action("Update job", Description = "Update a job's global data")]
    public async Task EditJob(
        [ActionParameter] JobRequest input,
        [ActionParameter] EditJobBody body,
        [ActionParameter] [DataSource(typeof(VendorDataHandler))]
        string? vendorId,
        [ActionParameter] [DataSource(typeof(UserDataHandler))]
        string? userId)
    {
        var bodyDictionary = new Dictionary<string, object>
        {
            {
                "jobs", new[] { new { uid = input.JobUId } }
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

        if (vendorId != null)
        {
            bodyDictionary.Add("providers", new[] { new { type = "VENDOR", id = vendorId } });
        }

        if (userId != null)
        {
            bodyDictionary.Add("providers", new[] { new { type = "USER", id = userId } });
        }

        var request = new RestRequest($"/api2/v3/jobs", Method.Patch)
            .WithJsonBody(bodyDictionary, JsonConfig.DateSettings);


        await Client.ExecuteWithHandling(request);
    }


    [Action("Download job target file", Description = "Download target file of a job")]
    public async Task<TargetFileResponse> DownloadTargetFile([ActionParameter] ProjectRequest projectRequest, [ActionParameter] JobRequest input)
    {
        var requestFile = new RestRequest($"/api2/v2/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/targetFile", Method.Put);
        var asyncRequest = await Client.PerformAsyncRequest(requestFile);

        var requestDownload = new RestRequest($"/api2/v2/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/downloadTargetFile/{asyncRequest.Id}?format={"ORIGINAL"}", Method.Get);
        var responseDownload = await Client.ExecuteWithHandling(requestDownload);

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = Uri.UnescapeDataString(filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1]);
        string mimeType = "";
        if (MimeTypes.TryGetMimeType(filename, out mimeType))
            mimeType = MediaTypeNames.Application.Octet;

        using var stream = new MemoryStream(fileData);
        var file = await fileManagementClient.UploadAsync(stream, mimeType, filename);
        return new() { File = file };
    }

    [Action("Download job original file", Description = "Download original file of a job")]
    public async Task<TargetFileResponse> DownloadOriginalFile([ActionParameter] ProjectRequest projectRequest, [ActionParameter] JobRequest input)
    {
        var requestFile = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/original?format=ORIGINAL", Method.Get);

        var responseDownload = await Client.ExecuteWithHandling(requestFile);

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = Uri.UnescapeDataString(filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1]);

        using var stream = new MemoryStream(fileData);
        var file = await fileManagementClient.UploadAsync(stream, responseDownload.ContentType, filename);
        return new() { File = file };
    }

    [Action("Upload job target file", Description = "Upload and update target file of a job")]
    public Task UpdateTargetFile([ActionParameter] ProjectRequest projectRequest, [ActionParameter] JobRequest job, [ActionParameter] UpdateTargetFileInput input)
    {
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

        var fileBytes = fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        var request =
            new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/target", Method.Post)
                .AddHeader("Content-Disposition", $"filename*=UTF-8''{input.File.Name}")
                .AddHeader("Content-Type", "application/octet-stream")
                .AddHeader("Memsource", jsonPayload)
                .AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

        return Client.ExecuteWithHandling(request);
    }

    [Action("Download job bilingual file", Description = "Download bilingual file for a job")]
    public async Task<TargetFileResponse> DownloadBilingualFile(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input,
        [ActionParameter] BilingualRequest bilingualRequest)
    {
        var requestFile = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/bilingualFile", Method.Post);
        if (!String.IsNullOrEmpty(bilingualRequest.Format))
        {
            requestFile.AddQueryParameter("format", bilingualRequest.Format);
        }

        if (bilingualRequest.Preview != null && bilingualRequest.Preview is false)
        {
            requestFile.AddQueryParameter("preview", "false");
        }

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

        var responseDownload = await Client.ExecuteWithHandling(requestFile);

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = Uri.UnescapeDataString(filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1]);

        using var stream = new MemoryStream(fileData);
        var file = await fileManagementClient.UploadAsync(stream, responseDownload.ContentType, filename);
        return new() { File = file };
    }

    [Action("Upload job bilingual file", Description = "Upload bilingual file to update job")]
    public Task UploadBilingualFile([ActionParameter] UploadBilingualFileRequest input)
    {
        var fileBytes = fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        var request = new RestRequest($"/api2/v2/bilingualFiles", Method.Post);
        if (!String.IsNullOrEmpty(input.saveToTransMemory))
        {
            request.AddQueryParameter("saveToTransMemory", input.saveToTransMemory);
        }

        if (input.setCompleted != null && input.setCompleted is true)
        {
            request.AddQueryParameter("setCompleted", true);
        }

        request.AlwaysMultipartFormData = true;
        request.AddFile("file", fileBytes, input.File.Name);

        return Client.ExecuteWithHandling(request);
    }


    [Action("Pre-translate job", Description = "Pre-translate a job in the project")]
    public async Task PreTranslateJob(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] PreTranslateJobInput input,
        [ActionParameter] PreTranslateSettings settings)
    {
        var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/jobs/preTranslate";
        var request = new RestRequest(endpoint, Method.Post);

        var body = new
        {
            jobs = input.Jobs.Select(uid => new { uid }),
            segmentFilters = input.SegmentFilters?.Any() == true ? input.SegmentFilters : new[] { "NOT_LOCKED" },
            useProjectPreTranslateSettings = input.UseProjectPreTranslateSettings ?? false,
            preTranslateSettings = settings
        };

        request.WithJsonBody(body, JsonConfig.Settings);
        await Client.PerformAsyncRequest(request);
    }

    [Action("Upload job source file", Description = "Upload and update the job source file in the project")]
    public async Task<UpdateSourceResponse> UpdateSource(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] UpdateSourceRequest input)
    {
        if (input.Jobs == null)
            throw new PluginMisconfigurationException("Job IDs is null. Please check the input and try again");
        if (!input.Jobs.Any())
            throw new PluginMisconfigurationException("No Job IDs provided");

        if (input.File == null)
            throw new PluginMisconfigurationException("File is null. Please check the input and try again");


        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/source", Method.Post);
        var jobs = input.Jobs.Select(u => new { uid = u });

        var output = JsonConvert.SerializeObject(new
        {
            preTranslate = input.preTranslate ?? false,
            jobs
        });

        var headers = new Dictionary<string, string>()
        {
            { "Memsource", output },
            { "Content-Disposition", $"filename*=UTF-8''{input.File.Name}" },
            { "Content-Type", "application/octet-stream" },
        };
        headers.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

        var fileResult = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileResult.GetByteData();

        if (fileBytes == null)
            throw new PluginMisconfigurationException("The file is empty. Please check the input and try again");

        request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

        var response = await Client.ExecuteWithHandling<ResponseWrapper<IEnumerable<UpdateSourceResponse>>>(request);

        return response.Content.First();
    }

}