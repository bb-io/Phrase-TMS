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

    [Action("Insert segment into memory", Description = "Insert a new segment into the translation memory")]
    public Task InsertSegment([ActionParameter] InsertSegmentRequest input)
    {
        var request = new RestRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}/segments", Method.Post);

        request.WithJsonBody(new
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
}