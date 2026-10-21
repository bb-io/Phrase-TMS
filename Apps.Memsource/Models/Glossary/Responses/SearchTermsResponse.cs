using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Glossary.Responses;

public class SearchTermsResponse
{
    [Display("Search results")]
    public IEnumerable<TermSearchResultDto> SearchResults { get; set; } = Array.Empty<TermSearchResultDto>();

    [Display("Exact match found")]
    public bool ExactMatchFound { get; set; }

    [Display("Matched term ID")]
    public string? MatchedTermId { get; set; }

    [Display("Matched term")]
    public TermDto? MatchedTerm { get; set; }
}

public class TermSearchResultDto
{
    [Display("Concept ID")]
    public string? ConceptId { get; set; }

    [Display("Source term")]
    public TermDto? SourceTerm { get; set; }

    [Display("Translation terms")]
    public IEnumerable<TermDto> TranslationTerms { get; set; } = Array.Empty<TermDto>();

    [Display("Term base")]
    public TermBaseDto? TermBase { get; set; }
}
