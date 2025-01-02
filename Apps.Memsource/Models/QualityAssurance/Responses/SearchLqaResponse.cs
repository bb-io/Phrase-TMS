using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class SearchLqaResponse
{
    [Display("Language quality assessments")]
    public List<LqaResponse> LanguageQualityAssessments { get; set; } = new();
}