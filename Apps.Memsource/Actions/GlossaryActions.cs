using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using System.Net.Mime;
using Apps.PhraseTMS.Models.Glossary.Requests;
using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Apps.PhraseTMS.Models.Glossary.Responses;

namespace Apps.PhraseTMS.Actions;

[ActionList("Glossaries")]
public class GlossaryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : PhraseInvocable(invocationContext)
{
    [Action("Create glossary", Description = "Create a new glossary")]
    public async Task<CreateGlossaryResponse> CreateGlossary(
        [ActionParameter] CreateGlossaryRequest input)
    {
        var request = new RestRequest("/api2/v1/termBases", Method.Post);
        request.WithJsonBody(new
        {
            name = input.Name,
            langs = input.Languages.ToArray(),
        });
        var glossaryDto = await Client.ExecuteWithHandling<GlossaryDto>(request);
        return new()
        {
            GlossaryId = glossaryDto.UId
        };
    }

    [Action("Export glossary", Description = "Export glossary")]
    public async Task<ExportGlossaryResponse> ExportGlossary([ActionParameter] ExportGlossaryRequest input)
    {
        var endpointGlossaryData = $"/api2/v1/termBases/{input.GlossaryUId}/export";
        var requestGlossaryData = new RestRequest(endpointGlossaryData, Method.Get);
        var responseGlossaryData = await Client.ExecuteAsync(requestGlossaryData);

        var endpointGlossaryDetails = $"/api2/v1/termBases/{input.GlossaryUId}";
        var requestGlossaryDetails = new RestRequest(endpointGlossaryDetails, Method.Get);
        var responseGlossaryDetails = await Client.ExecuteWithHandling<GlossaryDto>(requestGlossaryDetails);

        using var streamGlossaryData = new MemoryStream(responseGlossaryData.RawBytes ?? []);

        using var resultStream = await CoreTbxVersionsConverter.ConvertFromTbxV2ToV3(streamGlossaryData, responseGlossaryDetails.Name);
        return new() { File = await fileManagementClient.UploadAsync(resultStream, MediaTypeNames.Application.Xml, $"{responseGlossaryDetails.Name}.tbx") };
    }

    [Action("Import glossary", Description = "Import glossary")]
    public async Task ImportGlossary([ActionParameter] ImportGlossaryRequest input)
    {
        var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var fileTbxv2Stream = await CoreTbxVersionsConverter.ConvertFromTbxV3ToV2(fileStream);

        var endpointGlossaryData = $"/api2/v1/termBases/{input.GlossaryUId}/upload";
        var requestGlossaryData = new RestRequest(endpointGlossaryData.WithQuery(new{updateTerms = false}), Method.Post);
        requestGlossaryData.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.File.Name}");
        requestGlossaryData.AddParameter("application/octet-stream", fileTbxv2Stream.GetByteData().Result, ParameterType.RequestBody);

        await Client.ExecuteWithHandling(requestGlossaryData);
    }
}