using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class ListLQAProfilesResponse
{
    public IEnumerable<LQAProfileDto> Profiles { get; set; }
}