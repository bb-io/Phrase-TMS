using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.TranslationMemories.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.TranslationMemories.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.PhraseTMS.Dtos.Async;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Filters.Transformations;

namespace Apps.PhraseTMS.Actions;

[ActionList("Translation memory")]
public class TranslationMemoryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Search translation memories", Description = "Get all translation memories that match search criteria")]
    public Task<List<TranslationMemoryDto>> SearchTranslationMemories([ActionParameter] SearchTranslationMemoryRequest input)
    {
        var request = new RestRequest("/api2/v1/transMemories", Method.Get);

        if (!string.IsNullOrEmpty(input.Name))
            request.AddQueryParameter("name", input.Name);
        if (!string.IsNullOrEmpty(input.SourceLang))
            request.AddQueryParameter("sourceLang", input.SourceLang);
        if (!string.IsNullOrEmpty(input.TargetLang))
            request.AddQueryParameter("targetLang", input.TargetLang);
        if (!string.IsNullOrEmpty(input.ClientId))
            request.AddQueryParameter("clientId", input.ClientId);
        if (!string.IsNullOrEmpty(input.DomainId))
            request.AddQueryParameter("domainId", input.DomainId);
        if (!string.IsNullOrEmpty(input.SubDomainId))
            request.AddQueryParameter("subDomainId", input.SubDomainId);
        if (!string.IsNullOrEmpty(input.BusinessUnitId))
            request.AddQueryParameter("businessUnitId", input.BusinessUnitId);

        return Client.Paginate<TranslationMemoryDto>(request);
    }

    [Action("Create translation memory", Description = "Create a new translation memory")]
    public Task<TranslationMemoryDto> CreateTranslationMemory([ActionParameter] CreateTranslationMemoryRequest input)
    {
        var request = new RestRequest($"/api2/v1/transMemories", Method.Post);
        request.WithJsonBody(new
        {
            name = input.Name,
            sourceLang = input.SourceLang,
            targetLangs = input.TargetLang,
            note = input.Note
        });

        return Client.ExecuteWithHandling<TranslationMemoryDto>(request);
    }

    [Action("Get translation memory", Description = "Get specific translation memory")]
    public Task<TranslationMemoryDto> GetTranslationMemory([ActionParameter] GetTranslationMemoryRequest input)
    {
        var request = new RestRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}", Method.Get);
        return Client.ExecuteWithHandling<TranslationMemoryDto>(request);
    }

    [Action("Import TMX file", Description = "Imports a new TMX file into the TM")]
    public async Task ImportTmx([ActionParameter] ImportTmxRequest input, [ActionParameter] ImportTmxQuery query)
    {
        var endpoint = $"/api2/v2/transMemories/{input.TranslationMemoryUId}/import";
        var request = new RestRequest(endpoint.WithQuery(query), Method.Post);

        request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.File.Name}");
        request.AddHeader("Content-Type", "application/octet-stream");

        var fileBytes = fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

        await Client.PerformAsyncRequest(request);
    }

    [Action("Export translation memory", Description = "Export selected translation memory as either a TMX or an XLSX")]
    public async Task<ExportTranslationMemoryResponse> ExportTranslationMemory([ActionParameter] ExportTransMemoryRequest input,[ActionParameter] ExportTransMemoryBody body)
    {
        var request = new RestRequest($"/api2/v2/transMemories/{input.TranslationMemoryUId}/export", Method.Post);
        request.WithJsonBody(body,  JsonConfig.Settings);

        var asyncRequest = await Client.PerformAsyncRequest(request)!;

        var requestDownload = new RestRequest($"/api2/v1/transMemories/downloadExport/{asyncRequest.Id}?format={input.FileFormat}", Method.Get);
        var responseDownload = await Client.ExecuteWithHandling(requestDownload);

        using var stream = new MemoryStream(responseDownload.RawBytes!);
        var file = await fileManagementClient.UploadAsync(stream, responseDownload.ContentType, $"{input.TranslationMemoryUId}.tmx");
        return new() { File = file };
    }
    
    [Action("Update TM (insert segments from xliff)", Description = "Update TM by inserting segments from a xliff file")]
    public async Task UpdateTmInsertSegmentsFromFile([ActionParameter] UpdateTmRequest updateTmRequest)
    {
        var fileStream = await fileManagementClient.DownloadAsync(updateTmRequest.File);
        using var stream = new MemoryStream();
        await fileStream.CopyToAsync(stream);
        stream.Position = 0;
        
        var transformation = await Transformation.Parse(stream, updateTmRequest.File.Name);
        foreach (var segment in transformation.GetSegments())
        {
            var insertRequest = new InsertSegmentRequest
            {
                TranslationMemoryUId = updateTmRequest.TranslationMemoryUId,
                SourceSegment = segment.GetSource(),
                TargetSegment = segment.GetTarget(),
                TargetLanguage = updateTmRequest.TargetLanguage ?? transformation.TargetLanguage ?? string.Empty
            };
            
            await InsertSegmentAsync(updateTmRequest.TranslationMemoryUId, segment.Id, insertRequest.SourceSegment, insertRequest.TargetSegment, insertRequest.TargetLanguage);
        }
    }

    [Action("Insert segment into memory", Description = "Insert a new segment into the translation memory")]
    public Task InsertSegment([ActionParameter] InsertSegmentRequest input)
    {
        var request = new RestRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}/segments", Method.Post)
            .WithJsonBody(new
            {
                sourceSegment = input.SourceSegment,
                targetSegment = input.TargetSegment,
                targetLang = input.TargetLanguage,
                previousSourceSegment = input.PreviousSourceSegment,
                nextSourceSegment = input.NextSourceSegment,
            });
        
        return Client.ExecuteWithHandling(request);
    }

    [Action("Delete translation memory", Description = "Delete translation memory by ID")]
    public Task DeleteTransMemory([ActionParameter] DeleteTransMemoryRequest input)
    {
        var endpoint = $"/api2/v1/transMemories/{input.TranslationMemoryUId}";

        if (input.Purge is not null)
            endpoint += $"?purge={input.Purge}";

        var request = new RestRequest(endpoint, Method.Delete);
        return Client.ExecuteWithHandling(request);
    }


    [Action("Edit translation memories", Description = "Edit translation memories")]
    public Task<EditProjectTransMemoriesResponse> EditProjectTranslationMemories(
    [ActionParameter] ProjectRequest project,
    [ActionParameter] EditProjectTransMemoriesRequest input)
    {
        if (input.TransMemoryUids == null || input.TransMemoryUids.Length == 0)
            throw new PluginMisconfigurationException("Provide at least one TM UID.");

        if (input.Penalties != null && input.Penalties.Any(p => p < 0 || p > 100))
            throw new PluginMisconfigurationException("Penalties must be between 0 and 100.");

        var request = new RestRequest($"/api2/v3/projects/{project.ProjectUId}/transMemories", Method.Put);

        var count = input.TransMemoryUids.Length;
        var transMemories = Enumerable.Range(0, count).Select(i => new
        {
            transMemory = new { uid = input.TransMemoryUids[i] },
            readMode = GetAt(input.ReadModes, i),
            writeMode = GetAt(input.WriteModes, i),
            penalty = GetAt(input.Penalties, i),
            applyPenaltyTo101Only = GetAt(input.ApplyPenaltyTo101Only, i),
            order = GetAt(input.Orders, i)
        });

        var body = new
        {
            dataPerContext = new[]
            {
            new
            {
                transMemories,
                targetLang = input.TargetLanguage,
                workflowStep = string.IsNullOrWhiteSpace(input.WorkflowStepUid) ? null : new { uid = input.WorkflowStepUid },
                orderEnabled = input.OrderEnabled
            }
        }
        };

        request.WithJsonBody(body, JsonConfig.Settings);
        return Client.ExecuteWithHandling<EditProjectTransMemoriesResponse>(request);

        static T? GetAt<T>(T[]? arr, int index) => (arr != null && index < arr.Length) ? arr[index] : default;
    }

    private async Task InsertSegmentAsync(string translationMemoryUId, string? segmentId, string source, string target, string targetLanguage)
    {
        try
        {
            var request = new RestRequest($"/api2/v1/transMemories/{translationMemoryUId}/segments", Method.Post)
                .WithJsonBody(new
                {
                    sourceSegment = source,
                    targetSegment = target,
                    targetLang = targetLanguage
                });
        
            await Client.ExecuteWithHandling(request);
        }
        catch (Exception e)
        {
            throw new PluginApplicationException($"Failed to insert segment {(segmentId != null ? $"with ID {segmentId} " : string.Empty)}into TM {translationMemoryUId}. " +
                                                 $"Error: {e.Message}");
        }
    }
}