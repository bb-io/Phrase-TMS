using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Glossary.Requests;

public class ClearGlossaryRequest
{
    [Display("Glossary ID", Description = "Existing glossary for import")]
    [DataSource(typeof(TermBaseDataHandler))]
    public string GlossaryUId { get; set; }
}
