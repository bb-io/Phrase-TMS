using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Glossary.Requests;

public class SearchTermsRequest
{
    [Display("Term base ID")]
    [DataSource(typeof(TermBaseDataHandler))]
    public string TermBaseUId { get; set; } = string.Empty;

    [Display("Search query")]
    public string Query { get; set; } = string.Empty;

    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLanguage { get; set; } = string.Empty;

    [Display("Target languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string> TargetLanguages { get; set; } = Array.Empty<string>();

    [StaticDataSource(typeof(TermStatusDataHandler))]
    public string? Status { get; set; }
}
