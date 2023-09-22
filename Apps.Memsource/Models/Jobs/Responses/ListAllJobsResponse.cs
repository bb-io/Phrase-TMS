using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Jobs.Responses;

public class ListAllJobsResponse
{
    public IEnumerable<JobDto> Jobs { get; set; }
}