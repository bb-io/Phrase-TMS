using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Glossary.Requests;

public class ExportGlossaryRequest
{
    [Display("Glossary ID")]
    [DataSource(typeof(TermBaseDataHandler))]
    public string GlossaryUId { get; set; }
}