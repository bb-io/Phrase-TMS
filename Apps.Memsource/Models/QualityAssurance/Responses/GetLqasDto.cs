namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class GetLqasDto
{
    public List<LqaResponse> AssessmentDetails { get; set; } = new();
}