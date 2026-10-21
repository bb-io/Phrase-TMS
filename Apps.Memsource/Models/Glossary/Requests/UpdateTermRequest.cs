using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Glossary.Requests;

public class UpdateTermRequest
{
    [Display("Term base ID")]
    [DataSource(typeof(TermBaseDataHandler))]
    public string TermBaseUId { get; set; } = string.Empty;

    [Display("Term ID")]
    public string TermId { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    [Display("Language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string? Language { get; set; }

    [Display("Allow duplicate", Description = "Phrase defaults this value to true.")]
    public bool? AllowDuplicate { get; set; }

    [Display("Case sensitive")]
    public bool? CaseSensitive { get; set; }

    [Display("Exact match")]
    public bool? ExactMatch { get; set; }

    public bool? Forbidden { get; set; }

    public bool? Preferred { get; set; }

    public string? Note { get; set; }

    public string? Usage { get; set; }

    [Display("Short translation")]
    public string? ShortTranslation { get; set; }

    [StaticDataSource(typeof(TermStatusDataHandler))]
    public string? Status { get; set; }

    [StaticDataSource(typeof(TermGenderDataHandler))]
    public string? Gender { get; set; }

    [StaticDataSource(typeof(TermNumberDataHandler))]
    public string? Number { get; set; }

    [Display("Part of speech")]
    [StaticDataSource(typeof(TermPartOfSpeechDataHandler))]
    public string? PartOfSpeech { get; set; }

    [Display("Term type")]
    [StaticDataSource(typeof(TermTypeDataHandler))]
    public string? TermType { get; set; }

    [Display("Custom fields JSON", Description = "Optional JSON array of objects with fieldName and value properties.")]
    public string? CustomFieldsJson { get; set; }
}
