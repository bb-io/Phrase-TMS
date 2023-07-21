using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Responses
{
    public class JobResponseWrapper
    {
        public IEnumerable<JobDto> Jobs { get; set; }
    }
}
