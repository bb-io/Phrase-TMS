using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Glossary.Requests;

public class CreateTermRequest
{
    [Display("Term base ID")]
    [DataSource(typeof(TermBaseDataHandler))]
    public string TermBaseUId { get; set; } = string.Empty;

    [Display("Language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string Language { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    [Display("Concept ID", Description = "Assign the term to an existing concept. If omitted, Phrase creates a new concept.")]
    public string? ConceptId { get; set; }

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
