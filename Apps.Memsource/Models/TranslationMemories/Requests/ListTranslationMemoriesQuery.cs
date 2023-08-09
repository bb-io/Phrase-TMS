using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class ListTranslationMemoriesQuery
{
    [Display("Name")]
    public string? Name { get; set; }

    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? SourceLang { get; set; }

    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? TargetLang { get; set; }

    [Display("Client ID")]
    public string? ClientId { get; set; }

    [Display("Domain ID")]
    public string? DomainId { get; set; }

    [Display("Subdomain ID")]
    public string? SubDomainId { get; set; }

    [Display("Business unit ID")]
    public string? BusinessUnitId { get; set; }
}