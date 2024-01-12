using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Apps.PhraseTMS.Models.Glossary.Response;
using Apps.PhraseTMS.Models.Glossary.Requests;
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Glossaries.Utils.Parsers;
using Blackbird.Applications.Sdk.Glossaries.Utils.Dtos;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using System.Net.Http.Headers;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using DocumentFormat.OpenXml.Office2016.Excel;

namespace Apps.PhraseTMS.Actions
{
    [ActionList]
    public class GlossaryActions : BaseInvocable
    {
        private readonly IFileManagementClient _fileManagementClient;

        public GlossaryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(invocationContext)
        {
            _fileManagementClient = fileManagementClient;
        }

        [Action("Export glossary", Description = "Export glossary")]
        public async Task<ExportGlossaryResponse> ExportGlossary([ActionParameter] ExportGlossaryRequest input)
        {
            var client = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);

            var endpointGlossaryData = $"/api2/v1/termBases/{input.GlossaryUId}/export";
            var requestGlossaryData = new PhraseTmsRequest(endpointGlossaryData, Method.Get, InvocationContext.AuthenticationCredentialsProviders);
            var responseGlossaryData = await client.ExecuteAsync(requestGlossaryData);

            var endpointGlossaryDetails = $"/api2/v1/termBases/{input.GlossaryUId}";
            var requestGlossaryDetails = new PhraseTmsRequest(endpointGlossaryDetails, Method.Get, InvocationContext.AuthenticationCredentialsProviders);
            var responseGlossaryDetails = await client.ExecuteWithHandling<GlossaryDto>(requestGlossaryDetails);

            using var streamGlossaryData = new MemoryStream(responseGlossaryData.RawBytes);

            using var resultStream = await streamGlossaryData.ConvertFromTBXV2ToV3(responseGlossaryDetails.Name);
            return new ExportGlossaryResponse() { File = await _fileManagementClient.UploadAsync(resultStream, MediaTypeNames.Application.Xml, $"{responseGlossaryDetails.Name}.tbx") };
        }

        [Action("Import glossary", Description = "Import glossary")]
        public async Task ImportGlossary([ActionParameter] ImportGlossaryRequest input)
        {
            var client = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);

            var fileStream = await _fileManagementClient.DownloadAsync(input.File);
            var fileBytes = (await fileStream.ConvertFromTBXV3ToV2()).GetByteData();

            var endpointGlossaryData = $"/api2/v1/termBases/{input.GlossaryUId}/upload";
            var requestGlossaryData = new PhraseTmsRequest(endpointGlossaryData, Method.Post, InvocationContext.AuthenticationCredentialsProviders);
            requestGlossaryData.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.File.Name}");
            requestGlossaryData.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);

            await client.ExecuteAsync(requestGlossaryData);
        }

        [Action("Test conversion v3 to v2", Description = "Test conversion v3 to v2")]
        public async Task<ExportGlossaryResponse> TestConvGlossary([ActionParameter] ImportGlossaryRequest input)
        {
            var client = new PhraseTmsClient(InvocationContext.AuthenticationCredentialsProviders);

            var fileStream = await _fileManagementClient.DownloadAsync(input.File);
            var fileBytes = await fileStream.ConvertFromTBXV3ToV2();
            return new ExportGlossaryResponse() { File = await _fileManagementClient.UploadAsync(fileBytes, MediaTypeNames.Application.Xml, $"{input.File.Name}.tbx") };
        }
    }
}
