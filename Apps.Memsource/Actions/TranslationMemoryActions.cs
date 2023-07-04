﻿using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.TranslationMemories.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.TranslationMemories.Responses;
using Apps.PhraseTMS.Models.Async;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class TranslationMemoryActions
    {
        [Action("List translation memories", Description = "List translation memories")]
        public ListTranslationMemoriesResponse ListTranslationMemories(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories", Method.Get, authenticationCredentialsProviders);
            var response = client.Get<ResponseWrapper<List<TranslationMemoryDto>>>(request);
            return new ListTranslationMemoriesResponse()
            {
                Memories = response.Content
            };
        }

        [Action("Create translation memory", Description = "Create translation memory")]
        public TranslationMemoryDto CreateTranslationMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] CreateTranslationMemoryRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories", Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                name = input.Name,
                sourceLang = input.SourceLang,
                targetLangs = new[] { input.TargetLang }
            });
            return client.Execute<TranslationMemoryDto>(request).Data;
        }

        [Action("Get translation memory", Description = "Get translation memory")]
        public TranslationMemoryDto GetTranslationMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetTranslationMemoryRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}", Method.Get, authenticationCredentialsProviders);
            var response = client.Get<TranslationMemoryDto>(request);
            return response;
        }

        [Action("Import TMX file", Description = "Import TMX file")]
        public AsyncRequest ImportTmx(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ImportTmxRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v2/transMemories/{input.TranslationMemoryUId}/import",
                Method.Post, authenticationCredentialsProviders);

            request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.Filename}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);
            return client.PerformAsyncRequest(request, authenticationCredentialsProviders);
        }

        [Action("Export translation memory", Description = "Export translation memory")]
        public ExportTranslationMemoryResponse ExportTranslationMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ExportTransMemoryRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v2/transMemories/{input.TranslationMemoryUId}/export",
                Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new { });

            var asyncRequest = client.PerformAsyncRequest(request, authenticationCredentialsProviders);

            var requestDownload = new PhraseTmsRequest($"/api2/v1/transMemories/downloadExport/{asyncRequest.Id}?format={input.FileFormat}",
                Method.Get, authenticationCredentialsProviders);
            var responseDownload = client.Get(requestDownload);

            if (responseDownload == null) throw new Exception("Failed downloading target files");

            byte[] fileData = responseDownload.RawBytes;
            return new ExportTranslationMemoryResponse()
            {
                File = fileData
            };
        }

        [Action("Insert segment into memory", Description = "Insert segment into memory")]
        public void InsertSegment(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] InsertSegmentRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}/segments",
                Method.Post, authenticationCredentialsProviders);

            request.AddJsonBody(new
            {
                sourceSegment = input.SourceSegment,
                targetSegment = input.TargetSegment,
                targetLang = input.TargetLanguage
            });
            client.Execute(request);
        }

        [Action("Delete translation memory", Description = "Delete translation memory by UId")]
        public void DeleteTransMemory(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] DeleteTransMemoryRequest input)
        {
            var client = new PhraseTmsClient(authenticationCredentialsProviders);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}",
                Method.Delete, authenticationCredentialsProviders);
            client.Execute(request);
        }
    }
}
