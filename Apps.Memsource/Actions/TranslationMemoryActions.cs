﻿using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.TranslationMemories.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.TranslationMemories.Responses;
using Apps.PhraseTMS.Models.Async;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;

namespace Apps.PhraseTMS.Actions;

[ActionList]
public class TranslationMemoryActions
{
    private readonly IFileManagementClient _fileManagementClient;

    public TranslationMemoryActions(IFileManagementClient fileManagementClient)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("List translation memories", Description = "List all translation memories")]
    public async Task<ListTranslationMemoriesResponse> ListTranslationMemories(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ListTranslationMemoriesQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = "/api2/v1/transMemories";
        var request =
            new PhraseTmsRequest(endpoint.WithQuery(query), Method.Get, authenticationCredentialsProviders);

        var response = await client.Paginate<TranslationMemoryDto>(request);

        return new()
        {
            Memories = response
        };
    }

    [Action("Create translation memory", Description = "Create a new translation memory")]
    public Task<TranslationMemoryDto> CreateTranslationMemory(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] CreateTranslationMemoryRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/transMemories", Method.Post,
            authenticationCredentialsProviders);
        request.WithJsonBody(new
        {
            name = input.Name,
            sourceLang = input.SourceLang,
            targetLangs = input.TargetLang,
            note = input.Note
        });

        return client.ExecuteWithHandling<TranslationMemoryDto>(request);
    }

    [Action("Get translation memory", Description = "Get specific translation memory")]
    public Task<TranslationMemoryDto> GetTranslationMemory(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] GetTranslationMemoryRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}", Method.Get,
            authenticationCredentialsProviders);

        return client.ExecuteWithHandling<TranslationMemoryDto>(request);
    }

    [Action("Import TMX file", Description = "Import new TMX file")]
    public async Task<AsyncRequest?> ImportTmx(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ImportTmxRequest input,
        [ActionParameter] ImportTmxQuery query)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = $"/api2/v2/transMemories/{input.TranslationMemoryUId}/import";
        var request = new PhraseTmsRequest(endpoint.WithQuery(query),
            Method.Post, authenticationCredentialsProviders);

        request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.File.Name}");
        request.AddHeader("Content-Type", "application/octet-stream");

        var fileBytes = _fileManagementClient.DownloadAsync(input.File).Result.GetByteData().Result;
        request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

        return await client.PerformAsyncRequest(request, authenticationCredentialsProviders);
    }

    [Action("Export translation memory", Description = "Export selected translation memory")]
    public async Task<ExportTranslationMemoryResponse> ExportTranslationMemory(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] ExportTransMemoryRequest input,
        [ActionParameter] ExportTransMemoryBody body)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v2/transMemories/{input.TranslationMemoryUId}/export",
            Method.Post, authenticationCredentialsProviders);
        request.WithJsonBody(body,  JsonConfig.Settings);

        var asyncRequest = await client.PerformAsyncRequest(request, authenticationCredentialsProviders)!;

        var requestDownload = new PhraseTmsRequest(
            $"/api2/v1/transMemories/downloadExport/{asyncRequest.Id}?format={input.FileFormat}",
            Method.Get, authenticationCredentialsProviders);
        var responseDownload = await client.ExecuteWithHandling(requestDownload);

        if (responseDownload == null) throw new("Failed downloading target files");

        using var stream = new MemoryStream(responseDownload.RawBytes!);
        var file = await _fileManagementClient.UploadAsync(stream, responseDownload.ContentType, $"{input.TranslationMemoryUId}.tmx");
        return new() { File = file };
    }

    [Action("Insert segment into memory", Description = "Insert a new segment into the translation memory")]
    public Task InsertSegment(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] InsertSegmentRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var request = new PhraseTmsRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}/segments",
            Method.Post, authenticationCredentialsProviders);

        request.WithJsonBody(new
        {
            sourceSegment = input.SourceSegment,
            targetSegment = input.TargetSegment,
            targetLang = input.TargetLanguage,
            previousSourceSegment = input.PreviousSourceSegment,
            nextSourceSegment = input.NextSourceSegment,
        });
        return client.ExecuteWithHandling(request);
    }

    [Action("Delete translation memory", Description = "Delete translation memory by UId")]
    public Task DeleteTransMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        [ActionParameter] DeleteTransMemoryRequest input)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProviders);

        var endpoint = $"/api2/v1/transMemories/{input.TranslationMemoryUId}";

        if (input.Purge is not null)
            endpoint += $"?purge={input.Purge}";

        var request = new PhraseTmsRequest(endpoint, Method.Delete, authenticationCredentialsProviders);
        return client.ExecuteWithHandling(request);
    }
}