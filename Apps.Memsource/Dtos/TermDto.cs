using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class TermDto
{
    [Display("Term ID")]
    public string? Id { get; set; }

    public string Text { get; set; } = string.Empty;

    [Display("Language")]
    public string? Lang { get; set; }

    [Display("Concept ID")]
    public string? ConceptId { get; set; }

    [Display("Concept note")]
    public string? ConceptNote { get; set; }

    public string? Status { get; set; }

    [Display("Case sensitive")]
    public bool? CaseSensitive { get; set; }

    [Display("Exact match")]
    public bool? ExactMatch { get; set; }

    public bool? Forbidden { get; set; }

    public bool? Preferred { get; set; }

    public bool? Writable { get; set; }

    public string? Note { get; set; }

    public string? Usage { get; set; }

    [Display("Short translation")]
    public string? ShortTranslation { get; set; }

    public string? Gender { get; set; }

    public string? Number { get; set; }

    [Display("Part of speech")]
    public string? PartOfSpeech { get; set; }

    [Display("Term type")]
    public string? TermType { get; set; }

    public string? Definition { get; set; }

    public string? Domain { get; set; }

    public string? Url { get; set; }

    [Display("Created at")]
    public DateTime? CreatedAt { get; set; }

    [Display("Modified at")]
    public DateTime? ModifiedAt { get; set; }

    [Display("Custom fields")]
    public IEnumerable<TermCustomFieldDto>? CustomFields { get; set; }
}

public class TermCustomFieldDto
{
    [Display("Field name")]
    public string? FieldName { get; set; }

    public object? Value { get; set; }
}
