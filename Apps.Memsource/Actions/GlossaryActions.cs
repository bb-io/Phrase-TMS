using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Models.Glossary.Requests;
using Apps.PhraseTMS.Models.Glossary.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Glossaries.Utils.Converters;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;
using System.Net.Mime;
using System.Text;
using System.Xml.Linq;

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
        var responseGlossaryData = await Client.ExecuteWithHandling(requestGlossaryData);

        var endpointGlossaryDetails = $"/api2/v1/termBases/{input.GlossaryUId}";
        var requestGlossaryDetails = new RestRequest(endpointGlossaryDetails, Method.Get);
        var responseGlossaryDetails = await Client.ExecuteWithHandling<GlossaryDto>(requestGlossaryDetails);

        var tbx2Bytes = responseGlossaryData.RawBytes ?? [];

        using var tbx2Stream = new MemoryStream(tbx2Bytes);
        using var tbx3Stream = await CoreTbxVersionsConverter
            .ConvertFromTbxV2ToV3(tbx2Stream, responseGlossaryDetails.Name);

        using var finalStream = MergeFromTbx2IntoTbx3(tbx2Bytes, tbx3Stream);

        return new()
        {
            File = await fileManagementClient.UploadAsync(
                finalStream,
                MediaTypeNames.Application.Xml,
                $"{responseGlossaryDetails.Name}.tbx")
        };
    }

    [Action("Import glossary", Description = "Import glossary")]
    public async Task ImportGlossary([ActionParameter] ImportGlossaryRequest input)
    {
        var fileStream = await fileManagementClient.DownloadAsync(input.File);
        await using var fileTbxv2Stream = await CoreTbxVersionsConverter
            .ConvertFromTbxV3ToV2(fileStream, convertAll: true);

        var updateTerms = input.UpdateExistingTerms ?? false;
        var endpoint = $"/api2/v1/termBases/{input.GlossaryUId}/upload"
            .WithQuery(new { updateTerms });
        var bytes = await fileTbxv2Stream.GetByteData();

        var request = new RestRequest(endpoint, Method.Post);
        request.AddHeader("Content-Disposition", $"filename*=UTF-8''{input.File.Name}");
        request.AddParameter("application/octet-stream", bytes, ParameterType.RequestBody);

        await Client.ExecuteWithHandling(request);
    }

    private static Stream MergeFromTbx2IntoTbx3(byte[] tbx2Bytes, Stream tbx3Stream)
    {
        using var tbx2Ms = new MemoryStream(tbx2Bytes);
        var v2 = XDocument.Load(tbx2Ms);

        tbx3Stream.Position = 0;
        var v3 = XDocument.Load(tbx3Stream);
        XNamespace ns = "urn:iso:std:iso:30042:ed-2";

        var termEntries = v2.Descendants("termEntry").ToList();
        var conceptEntries = v3.Descendants(ns + "conceptEntry").ToList();

        for (int i = 0; i < termEntries.Count && i < conceptEntries.Count; i++)
        {
            var termEntry = termEntries[i];
            var conceptEntry = conceptEntries[i];

            var existingDescrips = conceptEntry.Elements(ns + "descrip").ToList();

            foreach (var descrip in termEntry.Elements("descrip"))
            {
                var type = (string?)descrip.Attribute("type");
                if (string.IsNullOrWhiteSpace(type))
                    continue;

                bool alreadyExists = existingDescrips
                    .Any(d => (string?)d.Attribute("type") == type);

                if (alreadyExists)
                    continue;

                var newDescrip = new XElement(ns + "descrip",
                    new XAttribute("type", type),
                    descrip.Value);

                var firstChild = conceptEntry.Elements().FirstOrDefault();
                if (firstChild == null)
                    conceptEntry.Add(newDescrip);
                else
                    firstChild.AddBeforeSelf(newDescrip);
            }

            foreach (var langSet in termEntry.Elements("langSet"))
            {
                var lang = (string?)langSet.Attribute(XNamespace.Xml + "lang");
                if (string.IsNullOrWhiteSpace(lang))
                    continue;

                var langSec = conceptEntry.Elements(ns + "langSec")
                    .FirstOrDefault(ls => (string?)ls.Attribute(XNamespace.Xml + "lang") == lang);

                if (langSec == null)
                {
                    langSec = new XElement(ns + "langSec",
                        new XAttribute(XNamespace.Xml + "lang", lang));
                    conceptEntry.Add(langSec);
                }

                foreach (var tig in langSet.Elements("tig"))
                {
                    var termText = tig.Element("term")?.Value;
                    if (string.IsNullOrWhiteSpace(termText))
                        continue;

                    var termSec = langSec.Elements(ns + "termSec")
                        .FirstOrDefault(ts => ts.Element(ns + "term")?.Value == termText);

                    if (termSec == null)
                    {
                        termSec = new XElement(ns + "termSec",
                            new XElement(ns + "term", termText));
                        langSec.Add(termSec);
                    }

                    foreach (var note in tig.Elements("note"))
                    {
                        bool noteExists = termSec.Elements(ns + "note")
                            .Any(n => n.Value == note.Value);

                        if (!noteExists)
                        {
                            termSec.Add(new XElement(ns + "note", note.Value));
                        }
                    }

                    foreach (var tn in tig.Elements("termNote"))
                    {
                        var tnType = (string?)tn.Attribute("type");
                        if (string.IsNullOrWhiteSpace(tnType))
                            continue;

                        var existingTn = termSec.Elements(ns + "termNote")
                            .FirstOrDefault(x => (string?)x.Attribute("type") == tnType);

                        if (existingTn == null)
                        {
                            termSec.Add(new XElement(ns + "termNote",
                                new XAttribute("type", tnType),
                                tn.Value));
                        }
                        else if (string.IsNullOrWhiteSpace(existingTn.Value))
                        {
                            existingTn.Value = tn.Value;
                        }
                    }
                }
            }
        }

        var outStream = new MemoryStream();
        v3.Save(outStream);
        outStream.Position = 0;
        return outStream;
    }
}