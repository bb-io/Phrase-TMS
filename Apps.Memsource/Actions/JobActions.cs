using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Dtos.Async;
using Apps.PhraseTMS.Dtos.Jobs;
using Apps.PhraseTMS.Models;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Jobs.Responses;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Extensions;
using Blackbird.Filters.Transformations;
using Blackbird.Filters.Xliff.Xliff2;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Apps.PhraseTMS.Actions;

[ActionList("Jobs")]
public class JobActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Search jobs", Description = "Returns a list of jobs in the project based on specified parameters")]
    public async Task<ListAllJobsResponse> ListAllJobs(
        [ActionParameter] ProjectRequest input,
        [ActionParameter] ListAllJobsQuery query,
        [ActionParameter] JobStatusesRequest jobStatusesRequest,
        [ActionParameter] WorkflowStepOptionalRequest workflowStepRequest,
        [ActionParameter] [Display("LQA Score null?")] bool? LQAScorenull,
        [ActionParameter] [Display("Last workflow step")] bool? LastWfStep)
    {
        if (string.IsNullOrEmpty(input.ProjectUId))
        {
            throw new PluginMisconfigurationException("Project ID is null or empty. Please check your input and try again");
        }

        if (LastWfStep.GetValueOrDefault() && string.IsNullOrWhiteSpace(input?.ProjectUId))
            throw new PluginMisconfigurationException("When 'Last workflow step' is enabled, you must provide a valid Project ID.");

        if (!string.IsNullOrWhiteSpace(query?.NoteContains) || !string.IsNullOrWhiteSpace(query?.NoteNotContains))
        {
            var projectNote = await GetProjectNote(input.ProjectUId!);
            var note = projectNote ?? string.Empty;

            bool pass = true;
            if (!string.IsNullOrWhiteSpace(query!.NoteContains))
                pass &= note.IndexOf(query.NoteContains, StringComparison.OrdinalIgnoreCase) >= 0;

            if (!string.IsNullOrWhiteSpace(query!.NoteNotContains))
                pass &= note.IndexOf(query.NoteNotContains, StringComparison.OrdinalIgnoreCase) < 0;

            if (!pass)
                return new ListAllJobsResponse { Jobs = Array.Empty<ListJobDto>() };
        }

        var endpoint = $"/api2/v2/projects/{input.ProjectUId}/jobs";
        var jobs = new List<ListJobDto>();
        int workflowLevel = 1;
        if (workflowStepRequest != null && !String.IsNullOrEmpty(workflowStepRequest.WorkflowStepId))
        {
            workflowLevel = await Client.GetWorkflowstepLevel(input.ProjectUId, workflowStepRequest.WorkflowStepId); 
        }
        else 
        {
            if (LastWfStep.HasValue && LastWfStep.Value)
            {
                workflowLevel = await Client.GetLastWorkflowstepLevel(input.ProjectUId);
            }
        }
        

        if (query != null && query.AssignedUsers?.Any() == true)
        {
            foreach (var userId in query.AssignedUsers)
            {
                var request = new RestRequest(endpoint, Method.Get);

                request.AddQueryParameter("assignedUser", userId);

                if (query.DueInHours.HasValue)
                {
                    request.AddQueryParameter("dueInHours", query.DueInHours.Value);
                }
                if (!string.IsNullOrEmpty(query.Filename))
                {
                    request.AddQueryParameter("filename", query.Filename);
                }
                if (!string.IsNullOrEmpty(query.TargetLang))
                {
                    request.AddQueryParameter("targetLang", query.TargetLang);
                }
                if (query.AssignedVendor.HasValue)
                {
                    request.AddQueryParameter("assignedVendor", query.AssignedVendor.Value);
                }

                request.AddQueryParameter("workflowLevel", workflowLevel);
                

                var response = await Client.Paginate<ListJobDto>(request);
                jobs.AddRange(response);
            }
        }
        else
        {
            var request = new RestRequest(endpoint, Method.Get);

            if (query != null)
            {
                if (query.DueInHours.HasValue)
                {
                    request.AddQueryParameter("dueInHours", query.DueInHours.Value);
                }
                if (!string.IsNullOrEmpty(query.Filename))
                {
                    request.AddQueryParameter("filename", query.Filename);
                }
                if (!string.IsNullOrEmpty(query.TargetLang))
                {
                    request.AddQueryParameter("targetLang", query.TargetLang);
                }
                if (query.AssignedVendor.HasValue)
                {
                    request.AddQueryParameter("assignedVendor", query.AssignedVendor.Value);
                }
            }

            request.AddQueryParameter("workflowLevel", workflowLevel);
            
            var response = await Client.Paginate<ListJobDto>(request);
            jobs.AddRange(response);
        }

        try
        {
            jobs = jobs
                .GroupBy(j => j.Uid)
                .Select(g => g.First())
                .ToList();

            if (LQAScorenull.HasValue && LQAScorenull.Value)
            {
                var lqaEndpoint = "/api2/v1/lqa/assessments";
                var lqaRequest = new RestRequest(lqaEndpoint, Method.Post);
                var bodyDictionary = new Dictionary<string, object>
            {
                { "jobParts", jobs.Select(u => new { uid = u.Uid }) }
            };

                lqaRequest.WithJsonBody(bodyDictionary);
                var result = await Client.ExecuteWithHandling(lqaRequest);
                var data = JsonConvert.DeserializeObject<LQAAssessmentSimpleDto>(result.Content!);
                var jobsWithoutLQA = data?.AssessmentDetails?
                    .Where(a => a.LqaEnabled && a.AssessmentResult == null)
                    .Select(a => a.JobPartUid)?.ToList() ?? Enumerable.Empty<string>();


                jobs = jobs.Where(x => jobsWithoutLQA.Contains(x.Uid))?.ToList() ?? new List<ListJobDto>();
                
            }

            if (jobStatusesRequest != null && jobStatusesRequest.Statuses?.Any() == true)
            {
                jobs = jobs.Where(x => jobStatusesRequest.Statuses.Contains(x.Status)).ToList();
            }

            return new ListAllJobsResponse
            {
                Jobs = jobs
            };
        }
        catch (Exception e)
        {
            if (e.Message.Contains("Invalid parameters") || e.Message.Contains("The object referenced by the field") ||
                e.Message.Contains("unsupported locale"))
            {
                throw new PluginMisconfigurationException(e.Message + " Make sure that the input values are correct.");
            }

            throw new PluginApplicationException(e.Message);
        }
    }

    [Action("Export jobs to online repository", Description = "Exports jobs to online repository")]
    public async Task<ExportJobsResponse> ExportJobsToOnlineRepository(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] ExportJobsToOnlineRepositoryRequest jobsRequest)
    {
        var request = new RestRequest($"/api2/v3/projects/{projectRequest.ProjectUId}/jobs/export", Method.Post);

        var body = new
        {
            jobs = jobsRequest.JobIds.Select(jobId => new { uid = jobId }).ToList()
        };

        request.AddJsonBody(body);
        Console.WriteLine(JsonConvert.SerializeObject(request, Formatting.Indented));
        var response = await Client.ExecuteWithHandling<ExportJobsResponse>(request);
        return response;
    }

    [Action("Get job", Description = "Get all job information for a specific job")]
    public async Task<JobDto> GetJob(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input
        )
    {
        if (string.IsNullOrWhiteSpace(projectRequest?.ProjectUId))
        {
            throw new PluginMisconfigurationException("Project Id cannot be null or empty. Please check your input and try again");
        }

        if (string.IsNullOrWhiteSpace(input?.JobUId))
        {
            throw new PluginMisconfigurationException("Job Id cannot be null or empty. Please check your input and try again");
        }

        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}", Method.Get);
        return await Client.ExecuteWithHandling<JobDto>(request);
    }

    [Action("Find job from source file ID", Description = "Given a source file ID, a workflow step ID and a language, returns the job.")]
    public async Task<JobDto> FindJob(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] [Display("Source file ID")] string sourceFileId,
        [ActionParameter] WorkflowStepRequest workflowStepRequest,
        [ActionParameter] TargetLanguageRequest targetLanguage
        )
    {
        var request = new RestRequest($"/api2/v2/projects/{projectRequest.ProjectUId}/jobs", Method.Get);
        var workflowLevel = await Client.GetWorkflowstepLevel(projectRequest.ProjectUId, workflowStepRequest.WorkflowStepId);
        request.AddQueryParameter("workflowLevel", workflowLevel);
        request.AddQueryParameter("targetLang", targetLanguage.TargetLang);

        try
        {
            var response = await Client.Paginate<ListJobDto>(request);
            var job = response.FirstOrDefault(x => x.SourceFileUid == sourceFileId);
            if (job == null) return new JobDto();

            var jobRequest = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{job.Uid}", Method.Get);

            return await Client.ExecuteWithHandling<JobDto>(jobRequest);
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

    [Action("Find job from server task ID", Description = "Given a server task ID, a workflow step ID and a project ID, returns the job.")]
    public async Task<JobDto> FindJobFromTask(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter][Display("Server task ID")] string serverTaskId,
        [ActionParameter] WorkflowStepRequest workflowStepRequest
        )
    {
        var request = new RestRequest($"/api2/v1/mappings/tasks/{serverTaskId}", Method.Get);
        var workflowLevel = await Client.GetWorkflowstepLevel(projectRequest.ProjectUId, workflowStepRequest.WorkflowStepId);
        request.AddQueryParameter("workflowLevel", workflowLevel);

        var response = await Client.ExecuteAsync(request);
        using var doc = JsonDocument.Parse(response?.Content);
        string jobUid = doc.RootElement.GetProperty("job").GetProperty("uid").GetString();

        var jobrequest = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobUid}", Method.Get);
        return await Client.ExecuteWithHandling<JobDto>(jobrequest);
    }

    // Should be removed in a couple of updates when people adjust.
    //[Action("Create job (use this instead: Upload source file)", Description = "Will be removed in a future version. Use 'Upload source file (create jobs)' instead.")]
    //public async Task<CreatedJobDto> CreateJob(
    //    [ActionParameter] ProjectRequest projectRequest,
    //    [ActionParameter] CreateJobRequest input)
    //{
    //    var fileName = input.File.Name;
    //    string fileNameForHeader = fileName;
    //    if (!IsOnlyAscii(fileName))
    //    {
    //        fileNameForHeader = Uri.EscapeDataString(fileName);
    //    }


    //    if (string.IsNullOrWhiteSpace(projectRequest.ProjectUId))
    //    {
    //        throw new PluginMisconfigurationException("Project ID is not provided. Please specify a valid Project ID.");
    //    }

    //    if (input.File == null)
    //    {
    //        throw new PluginMisconfigurationException("File is not provided. Please upload a file.");
    //    }

    //    var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs", Method.Post);

    //    var bodyJson = JsonConvert.SerializeObject(new
    //    {
    //        targetLangs = new List<string> { input.TargetLanguage },
    //        preTranslate = input.preTranslate ?? false,
    //        useProjectFileImportSettings = input.useProjectFileImportSettings ?? true,
    //        due = input.DueDate
    //    });

    //    request
    //     .AddHeader("Memsource", bodyJson)
    //     .AddHeader("Content-Disposition", $"filename*=UTF-8''{fileNameForHeader}")
    //     .AddHeader("Content-Type", "application/octet-stream");

    //    var fileStream = await fileManagementClient.DownloadAsync(input.File);
    //    using (var memoryStream = new MemoryStream())
    //    {
    //        await fileStream.CopyToAsync(memoryStream);
    //        memoryStream.Position = 0;

    //        if (memoryStream.ReadByte() == -1)
    //        {
    //            throw new PluginMisconfigurationException("The provided file is empty. Please check your file input and try again");
    //        }

    //        memoryStream.Position = 0;
    //        var fileBytes = memoryStream.ToArray();
    //        request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);
    //    }

    //    var jobs = await Client.ExecuteWithHandling<JobResponseWrapper>(request);
    //    return jobs.Jobs.FirstOrDefault();
    //}

    private const string BlackbirdImportSettingsName = "Blackbird XLIFF 2.x";

    private async Task<ImportSettingDto> GetBlackbirdImportSettings()
    {
        var request = new RestRequest($"/api2/v1/importSettings", Method.Get);
        var result = await Client.Paginate<ImportSettingDto>(request);

        var blackbirdImportSettings = result.FirstOrDefault(x => x.name == BlackbirdImportSettingsName);        

        if (blackbirdImportSettings is not null) return blackbirdImportSettings;

        var newImportRequest = new RestRequest($"/api2/v1/importSettings", Method.Post);
        var body = new
        {
            name = BlackbirdImportSettingsName,
            fileImportSettings = new
            {
                xlf2 = new
                {
                    preserveWhitespace = false,
                    skipImportRules = "state=final",
                    importAsConfirmedRules = "state=reviewed",
                }
            }
        };

        newImportRequest.AddJsonBody(body);
        return await Client.ExecuteWithHandling<ImportSettingDto>(newImportRequest);
    }

    [Action("Upload source file (create jobs)", Description = "Given a new file, create jobs for the different workflow steps and target languages")]
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

        if (input.TargetLanguages?.Any() != true)
        {
            var _projectRequest = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}", Method.Get);
            var project = await Client.ExecuteWithHandling<ProjectDto>(_projectRequest);
            input.TargetLanguages = project.TargetLangs;
        }        

        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs", Method.Post);

        var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileStream.GetByteData();

        if (fileBytes.Length == 0)
        {
            throw new PluginMisconfigurationException("The provided file is empty. Please check your file input and try again");
        }

        ImportSettingDto? settings = null;

        // Phrase TMS doesn't support xliff v2.1 or newer as of September 2025,
        // so we need to convert it to v2.0 if the user uploads a newer version.
        if (input.File.Name.EndsWith(".xlf", StringComparison.OrdinalIgnoreCase) ||
            input.File.Name.EndsWith(".xliff", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var fileContent = Encoding.UTF8.GetString(fileBytes);

                if (Xliff2Serializer.IsXliff2(fileContent) && Xliff2Serializer.TryGetXliffVersion(fileContent, out var version))
                {
                    if (version != "2.0")
                    {
                        var transformation = Transformation.Parse(fileContent, input.File.Name);
                        var xliffV20 = Xliff2Serializer.Serialize(transformation, Xliff2Version.Xliff20);
                        fileBytes = Encoding.UTF8.GetBytes(xliffV20);
                    }
                    settings = await GetBlackbirdImportSettings();
                }
            }
            catch (Exception)
            {
                // If deserialization fails, we pass file to Phrase TMS as is
            }
        }

        var memsourceHeader = JsonConvert.SerializeObject(
           new
           {
               targetLangs = input.TargetLanguages,
               preTranslate = input.preTranslate ?? false,
               useProjectFileImportSettings = settings is null ? input.useProjectFileImportSettings ?? true : false,
               due = input.DueDate?.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ssK"),
               importSettings = settings,
           },
           new JsonSerializerSettings
           {
               NullValueHandling = NullValueHandling.Ignore,
               StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
           });


        var rawName = (input.File.Name ?? "upload")
        .Replace("\n", string.Empty)
        .Replace("\r", string.Empty)
        .Trim();

        var asciiName = Regex.Replace(rawName, @"[^\x20-\x7E]", "_");
        if (string.IsNullOrWhiteSpace(asciiName)) asciiName = "upload";
        var quotedAscii = asciiName.Replace("\\", "\\\\").Replace("\"", "\\\"");

        var encodedName = Uri.EscapeDataString(rawName);

        request
            .AddHeader("Memsource", memsourceHeader)
            .AddHeader("Content-Disposition",
                $"attachment; filename=\"{quotedAscii}\"; filename*=UTF-8''{encodedName}");

        request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

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
    public async Task<JobDto> EditJob([ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest input,
        [ActionParameter] EditJobBody body,
        [ActionParameter] [Display("Assignee (vendor ID)")][DataSource(typeof(VendorDataHandler))]
        string? vendorId,
        [ActionParameter] [Display("Assignee (user ID)")][DataSource(typeof(UserDataHandler))]
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

        if (!String.IsNullOrEmpty(vendorId))
        {
            bodyDictionary.Add("providers", new[] { new { type = "VENDOR", id = vendorId } });
        }

        if (!String.IsNullOrEmpty(userId))
        {
            bodyDictionary.Add("providers", new[] { new { type = "USER", id = userId } });
        }

        var request = new RestRequest($"/api2/v3/jobs", Method.Patch)
            .WithJsonBody(bodyDictionary, JsonConfig.DateSettings);
        var response = await Client.ExecuteWithHandling<ApiResponse>(request);
        if (response.Errors != null && response.Errors.Count() > 0)
        {
            throw new PluginApplicationException(string.Join("; ", response.Errors.Select(e => $"{e.Message} (Code: {e.Code})")));
        }
        var job = await GetJob(projectRequest, input);
        return job;
    }


    [Action("Download job target file", Description = "Download target file of a job")]
    public async Task<TargetFileResponse> DownloadTargetFile([ActionParameter] ProjectRequest projectRequest, [ActionParameter] JobRequest input)
    {
        var requestFile = new RestRequest($"/api2/v2/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/targetFile", Method.Put);
        var asyncRequest = await Client.PerformAsyncRequest(requestFile);

        var requestDownload = new RestRequest($"/api2/v2/projects/{projectRequest.ProjectUId}/jobs/{input.JobUId}/downloadTargetFile/{asyncRequest.Id}?format={"ORIGINAL"}", Method.Get);
        var responseDownload = await Client.ExecuteWithHandling(requestDownload);

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload?.ContentHeaders?.FirstOrDefault(h => h.Name == "Content-Disposition");
        var filename = Uri.UnescapeDataString(filenameHeader?.Value?.ToString()?.Split(';')[1].Split("\'\'")[1] ?? "");

        var fileString = Encoding.UTF8.GetString(fileData ?? []);
        if (!Xliff2Serializer.IsXliff2(fileString))
        {
            if (!MimeTypes.TryGetMimeType(filename, out var mimeType))
                mimeType = MediaTypeNames.Application.Octet;

            using var stream = new MemoryStream(fileData ?? []);
            var file = await fileManagementClient.UploadAsync(stream, mimeType, filename);
            return new() { File = file };
        }

        var transformation = Transformation.Parse(fileString, filename);
        var (mxliffFileData, mxFileName, _) = await GetBilingualFile(projectRequest, input, new BilingualRequest { });
        var mxFileString = Encoding.UTF8.GetString(mxliffFileData ?? []);

        var mxTransformation = Transformation.Parse(mxFileString, mxFileName ?? filename + ".mxliff");

        foreach(var unit in transformation.GetUnits())
        {
            var mXliffUnit = MXLIFFHelper.FindMatchingMXLIFFUnit(unit, mxTransformation);
            if (mXliffUnit is null) continue;
            if (MXLIFFHelper.IsModified(mXliffUnit))
            {
                unit.Provenance.Translation.Tool = "Phrase TMS";

                var modifiedPerson = MXLIFFHelper.GetModifiedUser(mXliffUnit, mxTransformation);
                if (modifiedPerson is not null)
                {
                    unit.Provenance.Translation.Person = modifiedPerson.FullName;
                }

                if (MXLIFFHelper.IsConfirmed(mXliffUnit))
                {
                    foreach(var segment in unit.Segments)
                    {
                        segment.State = SegmentState.Translated;
                    }
                }
            }
        }

        return new ()
        {
            File = await fileManagementClient.UploadAsync(transformation.Serialize().ToStream(), MediaTypes.Xliff, transformation.XliffFileName)
        };
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
        var fileName = input.File.Name;
        var fileNameForHeader = fileName;

        if (!IsOnlyAscii(fileNameForHeader))
        {
            fileNameForHeader = Uri.EscapeDataString(fileNameForHeader);
        }

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
                .AddHeader("Content-Disposition", $"filename*=UTF-8''{fileNameForHeader}")
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
        var (fileData, fileName, contentType) = await GetBilingualFile(projectRequest, input, bilingualRequest);

        using var stream = new MemoryStream(fileData);
        var file = await fileManagementClient.UploadAsync(stream, contentType, fileName);
        return new() { File = file };
    }

    private async Task<(byte[]? fileData, string? fileName, string? contentType)> GetBilingualFile(
        ProjectRequest projectRequest, 
        JobRequest jobRequest, 
        BilingualRequest bilingualRequest)
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
                    uid = jobRequest.JobUId
                }
            },
        });

        requestFile.AddJsonBody(jsonBody);

        var responseDownload = await Client.ExecuteWithHandling(requestFile);

        var fileData = responseDownload.RawBytes;
        var filenameHeader = responseDownload.ContentHeaders.First(h => h.Name == "Content-Disposition");
        var fileName = Uri.UnescapeDataString(filenameHeader.Value.ToString().Split(';')[1].Split("\'\'")[1]);

        return (fileData, fileName, responseDownload.ContentType);
    }

    [Action("Upload job bilingual file", Description = "Upload bilingual file to update job")]
    public async Task UploadBilingualFile([ActionParameter] UploadBilingualFileRequest input)
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

        await Client.ExecuteWithHandling(request);
    }


    [Action("Pre-translate job", Description = "Pre-translate a job in the project")]
    public async Task PreTranslateJob(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] PreTranslateJobInput input,
        [ActionParameter] PreTranslateSettings settings)
    {
        var endpoint = $"/api2/v3/projects/{projectRequest.ProjectUId}/jobs/preTranslate";
        var request = new RestRequest(endpoint, Method.Post);


        var translationMemorySettings = new Dictionary<string, object>();
        if (settings.UseTranslationMemory.HasValue)
            translationMemorySettings["useTranslationMemory"] = settings.UseTranslationMemory.Value;
        if (settings.TranslationMemoryThreshold.HasValue)
            translationMemorySettings["translationMemoryThreshold"] = settings.TranslationMemoryThreshold.Value;
        if (settings.Confirm100PercentMatches.HasValue)
            translationMemorySettings["confirm100PercentMatches"] = settings.Confirm100PercentMatches.Value;
        if (settings.Confirm101PercentMatches.HasValue)
            translationMemorySettings["confirm101PercentMatches"] = settings.Confirm101PercentMatches.Value;
        if (settings.Lock100PercentMatchesTM.HasValue)
            translationMemorySettings["lock100PercentMatches"] = settings.Lock100PercentMatchesTM.Value;
        if (settings.Lock101PercentMatches.HasValue)
            translationMemorySettings["lock101PercentMatches"] = settings.Lock101PercentMatches.Value;

        var machineTranslationSettings = new Dictionary<string, object>();
        if (settings.UseMachineTranslation.HasValue)
            machineTranslationSettings["useMachineTranslation"] = settings.UseMachineTranslation.Value;
        if (!string.IsNullOrEmpty(settings.MachineTranslationBehavior))
            machineTranslationSettings["machineTranslationBehavior"] = settings.MachineTranslationBehavior;
        if (settings.Lock100PercentMatchesMT.HasValue)
            machineTranslationSettings["lock100PercentMatches"] = settings.Lock100PercentMatchesMT.Value;
        if (settings.ConfirmMatches.HasValue)
            machineTranslationSettings["confirmMatches"] = settings.ConfirmMatches.Value;
        if (settings.ConfirmMatchesThreshold.HasValue)
            machineTranslationSettings["confirmMatchesThreshold"] = settings.ConfirmMatchesThreshold.Value;
        if (settings.UseAltTransOnly.HasValue)
            machineTranslationSettings["useAltTransOnly"] = settings.UseAltTransOnly.Value;
        if (settings.MtSuggestOnlyTmBelow.HasValue)
            machineTranslationSettings["mtSuggestOnlyTmBelow"] = settings.MtSuggestOnlyTmBelow.Value;
        if (settings.MtSuggestOnlyTmBelowThreshold.HasValue)
            machineTranslationSettings["mtSuggestOnlyTmBelowThreshold"] = settings.MtSuggestOnlyTmBelowThreshold.Value;

        var nonTranslatableSettings = new Dictionary<string, object>();
        if (settings.PreTranslateNonTranslatables.HasValue)
            nonTranslatableSettings["preTranslateNonTranslatables"] = settings.PreTranslateNonTranslatables.Value;
        if (settings.Confirm100PercentMatchesNT.HasValue)
            nonTranslatableSettings["confirm100PercentMatches"] = settings.Confirm100PercentMatchesNT.Value;
        if (settings.Lock100PercentMatchesNT.HasValue)
            nonTranslatableSettings["lock100PercentMatches"] = settings.Lock100PercentMatchesNT.Value;

        var preTranslateSettings = new Dictionary<string, object>();
        if (settings.AutoPropagateRepetitions.HasValue)
            preTranslateSettings["autoPropagateRepetitions"] = settings.AutoPropagateRepetitions.Value;
        if (settings.ConfirmRepetitions.HasValue)
            preTranslateSettings["confirmRepetitions"] = settings.ConfirmRepetitions.Value;
        if (settings.SetJobStatusCompleted.HasValue)
            preTranslateSettings["setJobStatusCompleted"] = settings.SetJobStatusCompleted.Value;
        if (settings.SetJobStatusCompletedWhenConfirmed.HasValue)
            preTranslateSettings["setJobStatusCompletedWhenConfirmed"] = settings.SetJobStatusCompletedWhenConfirmed.Value;
        if (settings.SetProjectStatusCompleted.HasValue)
            preTranslateSettings["setProjectStatusCompleted"] = settings.SetProjectStatusCompleted.Value;
        if (settings.OverwriteExistingTranslations.HasValue)
            preTranslateSettings["overwriteExistingTranslations"] = settings.OverwriteExistingTranslations.Value;
        if (translationMemorySettings.Any())
            preTranslateSettings["translationMemorySettings"] = translationMemorySettings;
        if (machineTranslationSettings.Any())
            preTranslateSettings["machineTranslationSettings"] = machineTranslationSettings;
        if (nonTranslatableSettings.Any())
            preTranslateSettings["nonTranslatableSettings"] = nonTranslatableSettings;

        var body = new
        {
            jobs = input.Jobs.Select(uid => new { uid }),
            segmentFilters = input.SegmentFilters?.Any() == true ? input.SegmentFilters : new[] { "NOT_LOCKED" },
            useProjectPreTranslateSettings = input.UseProjectPreTranslateSettings ?? false,
            preTranslateSettings = preTranslateSettings.Any() ? preTranslateSettings : null
        };

        request.WithJsonBody(body, JsonConfig.Settings);
        await Client.PerformAsyncRequest(request);
    }

    [Action("Upload job source file", Description = "Upload and update the job source file in the project")]
    public async Task<UpdateSourceJob> UpdateSource(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] UpdateSourceRequest input)
    {
        if (string.IsNullOrWhiteSpace(projectRequest.ProjectUId))
            throw new PluginMisconfigurationException("Project ID is null or empty. Please check the input and try again");

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

        var response = await Client.ExecuteWithHandling<UpdateSourceResponse>(request);

        return response.Jobs.FirstOrDefault();
    }

    [Action("Get segments count", Description = "Get current segments counts for specified job")]
    public async Task<SegmentsCountsResultDto> GetSegmentsCount([ActionParameter] ProjectRequest projectRequest, 
        [ActionParameter] JobRequest input )
    {
        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/segmentsCount",Method.Post);

        var body = JsonConvert.SerializeObject(new
        {
            jobs = new[]
            {
                new
                {
                    uid = input.JobUId
                }
            },
        });
        request.AddJsonBody(body);

        var response = await Client.ExecuteWithHandling<GetSegmentsCountResponse>(request);
        return response.SegmentsCountsResults.First();
    }

    [Action("Get aggregated segments count (multiple jobs)", Description = "Get aggregated segment counts for specified jobs in a project")]
    public async Task<AggregatedSegmentsCountsResultDto> GetAggregatedSegmentsCount(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] ListAllJobsQuery query,
        [ActionParameter] JobStatusesRequest jobStatusesRequest,
        [ActionParameter] WorkflowStepOptionalRequest workflowStepRequest,
        [ActionParameter][Display("LQA Score null?")] bool? LQAScorenull,
        [ActionParameter][Display("Last workflow step")] bool? LastWfStep)
    {
        const int batchSize = 100;

        var inputJobs = await ListAllJobs(projectRequest, query, jobStatusesRequest, workflowStepRequest, LQAScorenull, LastWfStep);

        var aggregatedResult = new AggregatedSegmentsCountsResultDto
        {
            SegmentsCount = 0,
            CompletedSegmentsCount = 0,
            LockedSegmentsCount = 0,
            TranslatedSegmentsCount = 0,
            WordsCount = 0,
            AllConfirmed = true
        };

        for (int i = 0; i < inputJobs.Jobs.Count(); i += batchSize)
        {
            var batch = inputJobs.Jobs.Skip(i).Take(batchSize).ToList();

            var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/segmentsCount", Method.Post);
            var jobsBody = batch.Select(j => new { uid = j.Uid });
            request.AddJsonBody(new { jobs = jobsBody });

            var response = await Client.ExecuteWithHandling<GetSegmentsCountResponse>(request);

            foreach (var result in response.SegmentsCountsResults)
            {
                var counts = result.Counts;

                aggregatedResult.SegmentsCount += counts.SegmentsCount;
                aggregatedResult.CompletedSegmentsCount += counts.CompletedSegmentsCount;
                aggregatedResult.LockedSegmentsCount += counts.LockedSegmentsCount;
                aggregatedResult.TranslatedSegmentsCount += counts.TranslatedSegmentsCount;
                aggregatedResult.WordsCount += counts.WordsCount;

                if (counts.AllConfirmed != true)
                {
                    aggregatedResult.AllConfirmed = false;
                }
            }
        }

        return aggregatedResult;
    }


    [Action("Remove assigned providers from job", Description = "Removes assigned providers from job")]
    public async Task<JobDto> RemoveProvider(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] JobRequest jobRequest)
    {
        var requestGet = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}", Method.Get);
        var job = await Client.ExecuteWithHandling<JobDto>(requestGet);

        var requestRemove = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs/{jobRequest.JobUId}", Method.Put);

        var removeBody = new
        {
            status = job.Status,
            dateDue = job.DateDue,
            providers = new List<string>() 
        };

        requestRemove.AddJsonBody(removeBody);

        return await Client.ExecuteWithHandling<JobDto>(requestRemove);
    }


    public static bool IsOnlyAscii(string input)
    {
        return input.All(c => c <= 127);
    }

    private async Task<string?> GetProjectNote(string projectUid)
    {
        var req = new RestRequest($"/api2/v1/projects/{projectUid}", Method.Get);
        var resp = await Client.ExecuteWithHandling(req);
        if (string.IsNullOrWhiteSpace(resp.Content)) return null;

        try
        {
            var j = Newtonsoft.Json.Linq.JObject.Parse(resp.Content);
            return j["note"]?.ToString();
        }
        catch
        {
            return null;
        }
    }
}