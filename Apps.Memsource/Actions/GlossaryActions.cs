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

        //[Action("Import glossary", Description = "Import glossary")]
        //public async Task<NewGlossaryResponse> ImportGlossary([ActionParameter] ImportGlossaryRequest request)
        //{
        //    using var glossaryStream = await _fileManagementClient.DownloadAsync(request.File);
        //    var blackbirdGlossary = await glossaryStream.ConvertFromTBX();

        //    var glosseryValues = new List<KeyValuePair<string, string>>();
        //    foreach (var entry in blackbirdGlossary.ConceptEntries)
        //    {
        //        var langSection1 = entry.LanguageSections.ElementAt(0);
        //        var langSection2 = entry.LanguageSections.ElementAt(1);
        //        glosseryValues.Add(new(langSection1.Terms.First().Term, langSection2.Terms.First().Term));
        //    }
        //    var glossaryEntries = new GlossaryEntries(glosseryValues);

        //    var sourceLanguage = blackbirdGlossary.ConceptEntries.First().LanguageSections.ElementAt(0).LanguageCode;
        //    var targetLanguage = blackbirdGlossary.ConceptEntries.First().LanguageSections.ElementAt(1).LanguageCode;

        //    var result = await Client.CreateGlossaryAsync(request.Name ?? blackbirdGlossary.Title, sourceLanguage, targetLanguage, glossaryEntries);
        //    await Client.WaitUntilGlossaryReadyAsync(result.GlossaryId);
        //    return new NewGlossaryResponse
        //    {
        //        GossaryId = result.GlossaryId,
        //        Name = result.Name,
        //        SourceLanguageCode = result.SourceLanguageCode,
        //        TargetLanguageCode = result.TargetLanguageCode,
        //        EntryCount = result.EntryCount,
        //    };
        //}
    }
}
