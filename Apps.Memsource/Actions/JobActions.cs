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
using System.Text.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using Blackbird.Filters.Transformations;
using Blackbird.Filters.Xliff.Xliff2;
using Blackbird.Filters.Enums;

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
        if (LastWfStep.GetValueOrDefault() && string.IsNullOrWhiteSpace(input?.ProjectUId))
            throw new PluginMisconfigurationException("When 'Last workflow step' is enabled, you must provide a valid Project ID.");

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
    [Action("Create job (use this instead: Upload source file)", Description = "Will be removed in a future version. Use 'Upload source file (create jobs)' instead.")]
    public async Task<CreatedJobDto> CreateJob(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] CreateJobRequest input)
    {
        var fileName = input.File.Name;
        string fileNameForHeader = fileName;
        if (!IsOnlyAscii(fileName))
        {
            fileNameForHeader = Uri.EscapeDataString(fileName);
        }


        if (string.IsNullOrWhiteSpace(projectRequest.ProjectUId))
        {
            throw new PluginMisconfigurationException("Project ID is not provided. Please specify a valid Project ID.");
        }

        if (input.File == null)
        {
            throw new PluginMisconfigurationException("File is not provided. Please upload a file.");
        }

        var request = new RestRequest($"/api2/v1/projects/{projectRequest.ProjectUId}/jobs", Method.Post);

        var bodyJson = JsonConvert.SerializeObject(new
        {
            targetLangs = new List<string> { input.TargetLanguage },
            preTranslate = input.preTranslate ?? false,
            useProjectFileImportSettings = input.useProjectFileImportSettings ?? true,
            due = input.DueDate
        });

        request
         .AddHeader("Memsource", bodyJson)
         .AddHeader("Content-Disposition", $"filename*=UTF-8''{fileNameForHeader}")
         .AddHeader("Content-Type", "application/octet-stream");

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

        var memsourceHeader = JsonConvert.SerializeObject(new
        {
            targetLangs = input.TargetLanguages,
            preTranslate = input.preTranslate ?? false,
            useProjectFileImportSettings = input.useProjectFileImportSettings ?? true,
            due = input.DueDate
        });

        var fileNameForHeader = input.File.Name;
        if (!IsOnlyAscii(input.File.Name))
        {
            fileNameForHeader = Uri.EscapeDataString(input.File.Name)
                .Replace("\n", string.Empty)
                .Replace("\r", string.Empty);
        }

        request
         .AddHeader("Memsource", memsourceHeader)
         .AddHeader("Content-Disposition", $"filename*=UTF-8''{fileNameForHeader}")
         .AddHeader("Content-Type", "application/octet-stream");

        var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileStream.GetByteData();

        if (fileBytes.Length == 0)
        {
            throw new PluginMisconfigurationException("The provided file is empty. Please check your file input and try again");
        }

        // Phrase TMS doesn't support xliff v2.1 or newer as of September 2025,
        // so we need to convert it to v2.0 if the user uploads a newer version.
        if (input.File.Name.EndsWith(".xlf", StringComparison.OrdinalIgnoreCase) ||
            input.File.Name.EndsWith(".xliff", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var fileContent = System.Text.Encoding.UTF8.GetString(fileBytes);

                if (Xliff2Serializer.TryGetXliffVersion(fileContent, out var version)
                    && version != "2.0")
                {
                    var transformation = Transformation.Parse(fileContent, input.File.Name);
                    var xliffV20 = Xliff2Serializer.Serialize(transformation, Xliff2Version.Xliff20);
                    fileBytes = System.Text.Encoding.UTF8.GetBytes(xliffV20);
                }
            }
            catch (Exception)
            {
                // If deserialization fails, we pass file to Phrase TMS as is
            }
        }

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
    public async Task EditJob([ActionParameter] ProjectRequest projectRequest,
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

    [Action("Get segments count", Description = "Get current segments counts for specified jobs")]
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

}