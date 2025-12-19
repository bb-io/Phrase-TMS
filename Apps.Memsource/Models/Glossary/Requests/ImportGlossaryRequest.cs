using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Glossary.Requests;

public class ImportGlossaryRequest
{
    [Display("Glossary ID", Description = "Existing glossary for import")]
    [DataSource(typeof(TermBaseDataHandler))]
    public string GlossaryUId { get; set; }

    [Display("Glossary file", Description = "Glossary file exported from other Blackbird apps")]
    public FileReference File { get; set; }

    [Display("Update existing terms",
        Description = "If enabled, existing terms in the term base will be updated. " +
                      "If disabled or not set, new terms will be created.")]
    public bool? UpdateExistingTerms { get; set; }
}