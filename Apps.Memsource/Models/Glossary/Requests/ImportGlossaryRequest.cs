using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Glossary.Requests;

public class ImportGlossaryRequest
{
    [Display("Glossary UID", Description = "Existing glossary for import")]
    [DataSource(typeof(TermBaseDataHandler))]
    public string GlossaryUId { get; set; }

    [Display("Glossary file", Description = "Glossary file exported from other Blackbird apps")]
    public FileReference File { get; set; }
}