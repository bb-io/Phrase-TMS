﻿using Apps.PhraseTMS.Constants;
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

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class JobActions(IFileManagementClient fileManagementClient)
{
    [Action("Search jobs", Description = "Returns a list of jobs in the project based on specified parameters")]
    public async Task<ListAllJobsResponse> ListAllJobs(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest input,
        [ActionParameter] ListAllJobsQuery query,
        [ActionParameter] JobStatusesRequest jobStatusesRequest,
        [ActionParameter] [Display("LQA Score null?")] bool? LQAScorenull)
    {
        var credentialsProviders = authenticationCredentialsProviders as AuthenticationCredentialsProvider[] ?? authenticationCredentialsProviders.ToArray();
        
        var client = new PhraseTmsClient(credentialsProviders);

        var endpoint = $"/api2/v2/projects/{input.ProjectUId}/jobs";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get,
            credentialsProviders);
        try
        {
            var response = await client.Paginate<JobDto>(request);
            response.ForEach(x => x.Project = new()
            {
                UId = input.ProjectUId
            });

            if (LQAScorenull.HasValue && LQAScorenull.Value)
            {
                var lqaEndpoint = "/api2/v1/lqa/assessments";
                var lqaRequest = new PhraseTmsRequest(lqaEndpoint, Method.Post,
                    credentialsProviders);
                var bodyDictionary = new Dictionary<string, object>
                {
                    { "jobParts", response.Select(u => new { uid = u.Uid }) }
                };
                
                lqaRequest.WithJsonBody(bodyDictionary);
                var result = await client.ExecuteWithHandling(lqaRequest);
                var data = JsonConvert.DeserializeObject<LQAAssessmentSimpleDto>(result.Content!);
                var jobsWithoutLQA = data?.AssessmentDetails?
                    .Where(a => a.LqaEnabled && a.AssessmentResult == null)
                    .Select(a => a.JobPartUid)?.ToList() ?? Enumerable.Empty<string>();

                return new()
                {
                    Jobs = response.Where(x => jobsWithoutLQA.Contains(x.Uid))?.ToList() ?? new List<JobDto>()
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

        var Linguists = new List<UserDto>();
        if (response.providers != null)
        {
            foreach (var user in response.providers)
            {
                try
                {
                    var client2 = new PhraseTmsClient(authenticationCredentialsProviders);
                    var request2 = new PhraseTmsRequest($"/api2/v3/users/{user.uid}",
                        Method.Get, authenticationCredentialsProviders);

                    var userinfo = await client2.ExecuteWithHandling<UserDto>(request2);
                    Linguists.Add(userinfo);
                }
                catch (Exception e)
                {
                    Linguists.Add(new UserDto { UId = user.uid, Id = user.id });
                }
            }
        }

        return new()
        {
            Uid = response.Uid,
            Filename = response.Filename,
            TargetLanguage = response.TargetLang,
            Status = response.Status,
            ProjectUid = response.Project.UId,
            WordCount = response.WordsCount,
            SourceLanguage = response.SourceLang,
            AssignedTo = Linguists
        };
    }

    [Action("Create job", Description = "Create a new job")]
    public async Task<CreateJobResponse> CreateJob(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
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

        if (input.TargetLanguages == null || !input.TargetLanguages.Any())
        {
            var projectActions = new ProjectActions(fileManagementClient);
            var project = await projectActions.GetProject(authenticationCredentialsProviders, projectRequest);
            input.TargetLanguages = project.TargetLangs;
        }

        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs",
            Method.Post, authenticationCredentialsProviders);

        var output = JsonConvert.SerializeObject(new
        {
            targetLangs = input.TargetLanguages,
            preTranslate = input.preTranslate ?? false,
            useProjectFileImportSettings = input.useProjectFileImportSettings ?? true
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
    public async Task EditJob(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input,
        [ActionParameter] EditJobBody body,
        [ActionParameter] [DataSource(typeof(VendorDataHandler))]
        string? Vendor,
        [ActionParameter] [DataSource(typeof(UserDataHandler))]
        string? User)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

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

        if (Vendor != null)
        {
            var vendorId = await GetVendorId(authenticationCredentialsProviders, Vendor);
            bodyDictionary.Add("providers", new[] { new { type = "VENDOR", id = vendorId } });
        }

        if (User != null)
        {
            var userId = await GetUserId(authenticationCredentialsProviders, User);
            bodyDictionary.Add("providers", new[] { new { type = "USER", id = userId } });
        }

        var request = new PhraseTmsRequest($"/api2/v3/jobs",
                Method.Patch, authenticationCredentialsProviders)
            .WithJsonBody(bodyDictionary, JsonConfig.DateSettings);


        await client.ExecuteWithHandling(request);
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
        var file = await fileManagementClient.UploadAsync(stream, mimeType, filename);
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
        var file = await fileManagementClient.UploadAsync(stream, responseDownload.ContentType, filename);
        return new() { File = file };
    }

    [Action("Update job target file", Description = "Update target file of a job")]
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

        var fileBytes = fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        var request =
            new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/target", Method.Post, creds)
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

        var responseDownload = await client.ExecuteWithHandling(requestFile);

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var filename = Uri.UnescapeDataString(filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1]);

        using var stream = new MemoryStream(fileData);
        var file = await fileManagementClient.UploadAsync(stream, responseDownload.ContentType, filename);
        return new() { File = file };
    }

    [Action("Upload bilingual file", Description = "Upload bilingual file to update job")]
    public Task UploadBilingualFile(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] UploadBilingualFileRequest input)
    {
        var client = new PhraseTmsClient(creds);

        var fileBytes = fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        var request = new PhraseTmsRequest($"/api2/v2/bilingualFiles", Method.Post, creds);
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

        return client.ExecuteWithHandling(request);
    }


    [Action("Pre-translate job", Description = "Pre-translate a job in the project")]
    public async Task<PreTranslateJobResponse> PreTranslateJob(IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] PreTranslateJobInput input,
        [ActionParameter] PreTranslateSettings settings)
    {
        var client = new PhraseTmsClient(creds);

        var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/jobs/preTranslate";
        var request = new PhraseTmsRequest(endpoint, Method.Post, creds);

        var body = new
        {
            jobs = input.Jobs.Select(uid => new { uid }),
            segmentFilters = input.SegmentFilters?.Any() == true ? input.SegmentFilters : new[] { "NOT_LOCKED" },
            useProjectPreTranslateSettings = input.UseProjectPreTranslateSettings ?? false,
            callbackUrl = input.CallbackUrl ?? "",
            preTranslateSettings = settings
        };

        request.WithJsonBody(body, JsonConfig.Settings);

        var response = await client.ExecuteWithHandling<PreTranslateJobResponse>(request);
        return response;
    }

    [Action("Update job source", Description = "Update the job source in the project")]
    public async Task<UpdateSourceResponse> UpdateSource(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] UpdateSourceRequest input)
    {
        //TODO: Input checking

        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/source",
            Method.Post, authenticationCredentialsProviders);
        var jobs = input.Jobs.Select(u => new { uid = u });

        var output = JsonConvert.SerializeObject(new
        {
            preTranslate = input.preTranslate ?? false,
            jobs = jobs
        });

        var headers = new Dictionary<string, string>()
        {
            { "Memsource", output },
            { "Content-Disposition", $"filename*=UTF-8''{input.File.Name}" },
            { "Content-Type", "application/octet-stream" },
        };
        headers.ToList().ForEach(x => request.AddHeader(x.Key, x.Value));

        var fileBytes = fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

        var response = await client.ExecuteWithHandling<ResponseWrapper<IEnumerable<UpdateSourceResponse>>>(request);

        return response.Content.First();
    }
    private async Task<string> GetUserId(IEnumerable<AuthenticationCredentialsProvider> creds, string linguist)
    {
        var actions = new UserActions();
        var userDetails = await actions.GetUser(creds, new Models.Users.Requests.GetUserRequest { UserUId = linguist });
        var user = await actions.FindUser(creds,
            new Models.Users.Requests.ListAllUsersQuery { email = userDetails.Email.Replace("+", "%2B") });
        return user.Id;
    }

    private async Task<string> GetVendorId(IEnumerable<AuthenticationCredentialsProvider> creds, string vendor)
    {
        var actions = new VendorActions();
        var userDetails =
            await actions.GetVendor(creds, new Models.Vendors.Requests.GetVendorRequest { VendorId = vendor });
        return userDetails.Id;
    }

}