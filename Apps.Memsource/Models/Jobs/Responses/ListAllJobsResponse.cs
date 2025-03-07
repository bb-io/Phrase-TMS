using Apps.PhraseTMS.Dtos;
using Apps.PhraseTMS.Dtos.Jobs;

namespace Apps.PhraseTMS.Models.Jobs.Responses;

public class ListAllJobsResponse
{
    public IEnumerable<ListJobDto> Jobs { get; set; }
}