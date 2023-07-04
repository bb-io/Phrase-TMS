using Apps.PhraseTms.Dtos;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class ListAllJobsResponse
    {
        public IEnumerable<JobDto> Jobs { get; set; }
    }
}
