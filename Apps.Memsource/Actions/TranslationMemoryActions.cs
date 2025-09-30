using Apps.PhraseTMS.Constants;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Projects.Requests;
using Apps.PhraseTMS.Models.TranslationMemories.Requests;
using Apps.PhraseTMS.Models.TranslationMemories.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Transformations;
using RestSharp;

namespace Apps.PhraseTMS.Actions;

[ActionList("Translation memory")]
public class TranslationMemoryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Search TM", Description = "Get all translation memories that match search criteria")]
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

    [Action("Create TM", Description = "Create a new translation memory")]
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

    [Action("Get TM", Description = "Get specific translation memory")]
    public Task<TranslationMemoryDto> GetTranslationMemory([ActionParameter] GetTranslationMemoryRequest input)
    {
        var request = new RestRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}", Method.Get);
        return Client.ExecuteWithHandling<TranslationMemoryDto>(request);
    }

    [Action("Import TMX file into TM", Description = "Imports a new TMX file into a translation memory")]
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

    [Action("Export TM", Description = "Export selected translation memory as either a TMX or an XLSX")]
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
    
    [Action("Update TM from XLIFF", Description = "Update translation memory by inserting segments from a xliff file")]
    public async Task UpdateTmInsertSegmentsFromFile([ActionParameter] UpdateTmRequest updateTmRequest)
    {
        var fileStream = await fileManagementClient.DownloadAsync(updateTmRequest.File);
        var transformation = await Transformation.Parse(fileStream, updateTmRequest.File.Name);

        var targetLanguage = updateTmRequest.TargetLanguage ?? transformation.TargetLanguage;
        if (string.IsNullOrWhiteSpace(targetLanguage))
            throw new PluginMisconfigurationException("Target language must be specified either in the input or in the XLIFF file.");

        var segmentStates = updateTmRequest.SegmentStates?
            .Select(SegmentStateHelper.ToSegmentState)
            .Where(state => state is not null)
            .Select(s => s!.Value)
            .ToList();

        var units = transformation.GetUnits().ToList();

        for (var unitIndex = 0; unitIndex < units.Count; unitIndex++)
        {
            var unit = units[unitIndex];

            for (var segmentIndex = 0; segmentIndex < unit.Segments.Count; segmentIndex++)
            {
                var segment = unit.Segments[segmentIndex];

                if (string.IsNullOrEmpty(segment.GetSource()) || string.IsNullOrEmpty(segment.GetTarget()))
                    continue;

                if (segmentStates is not null && !segmentStates.Contains(segment.State ?? SegmentState.Initial))
                    continue;

                var previousSegment = GetSurroundingSegment(units, unitIndex, segmentIndex, true);
                var nextSegment = GetSurroundingSegment(units, unitIndex, segmentIndex, false);

                await InsertSegment(new InsertSegmentRequest
                {
                    TranslationMemoryUId = updateTmRequest.TranslationMemoryUId,
                    SourceSegment = segment.GetSource(),
                    TargetSegment = segment.GetTarget(),
                    TargetLanguage = targetLanguage,
                    PreviousSourceSegment = previousSegment?.GetSource(),
                    NextSourceSegment = nextSegment?.GetSource(),
                });
            }
        }
    }

    [Action("Insert text into TM", Description = "Insert a new segment into a translation memory")]
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

    [Action("Delete TM", Description = "Delete selected translation memory")]
    public Task DeleteTransMemory([ActionParameter] DeleteTransMemoryRequest input)
    {
        var endpoint = $"/api2/v1/transMemories/{input.TranslationMemoryUId}";

        if (input.Purge is not null)
            endpoint += $"?purge={input.Purge}";

        var request = new RestRequest(endpoint, Method.Delete);
        return Client.ExecuteWithHandling(request);
    }


    [Action("Edit TMs", Description = "Edit translation memories")]
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

    private static Segment? GetSurroundingSegment(List<Unit> units, int unitIndex, int segmentIndex, bool getPrevious)
    {
        var unit = units[unitIndex];

        if (unit is null)
            return null;

        if (getPrevious)
        {
            if (segmentIndex > 0)
                return unit.Segments.ElementAt(segmentIndex - 1);

            if (getPrevious && unitIndex > 0)
                return units[unitIndex - 1].Segments.Last();
        }

        if (getPrevious == false)
        {
            if (segmentIndex < unit.Segments.Count - 1)
                return unit.Segments.ElementAt(segmentIndex + 1);

            if (unitIndex < units.Count - 1)
                return units[unitIndex + 1].Segments.First();
        }

        return null;
    }
}