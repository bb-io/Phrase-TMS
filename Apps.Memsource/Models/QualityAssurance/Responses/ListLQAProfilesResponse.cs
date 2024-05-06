using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class ListLqaProfilesResponse
{
    public IEnumerable<LqaProfileDto> Profiles { get; set; }
}