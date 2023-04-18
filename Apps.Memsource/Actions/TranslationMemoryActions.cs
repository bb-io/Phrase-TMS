using Apps.PhraseTMS.Models.Vendors.Requests;
using Apps.PhraseTms;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using Apps.PhraseTMS.Models.TranslationMemories.Requests;
using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Analysis.Requests;
using Apps.PhraseTMS.Models.Analysis.Responses;
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Models.TranslationMemories.Responses;
using Newtonsoft.Json;
using Apps.PhraseTMS.Models.Async;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class TranslationMemoryActions
    {
        [Action("List translation memories", Description = "List translation memories")]
        public ListTranslationMemoriesResponse ListTranslationMemories(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<ResponseWrapper<List<TranslationMemoryDto>>>(request);
            return new ListTranslationMemoriesResponse()
            {
                Memories = response.Content
            };
        }

        [Action("Create translation memory", Description = "Create translation memory")]
        public TranslationMemoryDto CreateTranslationMemory(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] CreateTranslationMemoryRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories", Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new
            {
                name = input.Name,
                sourceLang = input.SourceLang,
                targetLangs = new[] { input.TargetLang }
            });
            return client.Execute<TranslationMemoryDto>(request).Data;
        }

        [Action("Get translation memory", Description = "Get translation memory")]
        public TranslationMemoryDto GetTranslationMemory(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetTranslationMemoryRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}", Method.Get, authenticationCredentialsProvider.Value);
            var response = client.Get<TranslationMemoryDto>(request);
            return response;
        }

        [Action("Import TMX file", Description = "Import TMX file")]
        public AsyncRequest ImportTmx(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] ImportTmxRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v2/transMemories/{input.TranslationMemoryUId}/import",
                Method.Post, authenticationCredentialsProvider.Value);

            request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.Filename}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddParameter("application/octet-stream", input.File, ParameterType.RequestBody);
            return client.PerformAsyncRequest(request, authenticationCredentialsProvider);
        }

        [Action("Export translation memory", Description = "Export translation memory")]
        public ExportTranslationMemoryResponse ExportTranslationMemory(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] ExportTransMemoryRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v2/transMemories/{input.TranslationMemoryUId}/export",
                Method.Post, authenticationCredentialsProvider.Value);
            request.AddJsonBody(new { });

            var asyncRequest = client.PerformAsyncRequest(request, authenticationCredentialsProvider);

            var requestDownload = new PhraseTmsRequest($"/api2/v1/transMemories/downloadExport/{asyncRequest.Id}?format={input.FileFormat}",
                Method.Get, authenticationCredentialsProvider.Value);
            var responseDownload = client.Get(requestDownload);

            if (responseDownload == null) throw new Exception("Failed downloading target files");

            byte[] fileData = responseDownload.RawBytes;
            return new ExportTranslationMemoryResponse()
            {
                File = fileData
            };
        }

        [Action("Insert segment into memory", Description = "Insert segment into memory")]
        public void InsertSegment(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] InsertSegmentRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}/segments",
                Method.Post, authenticationCredentialsProvider.Value);

            request.AddJsonBody(new
            {
                sourceSegment = input.SourceSegment,
                targetSegment = input.TargetSegment,
                targetLang = input.TargetLanguage
            });
            client.Execute(request);
        }

        [Action("Delete translation memory", Description = "Delete translation memory by UId")]
        public void DeleteTransMemory(string url, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] DeleteTransMemoryRequest input)
        {
            var client = new PhraseTmsClient(url);
            var request = new PhraseTmsRequest($"/api2/v1/transMemories/{input.TranslationMemoryUId}",
                Method.Delete, authenticationCredentialsProvider.Value);
            client.Execute(request);
        }
    }
}
