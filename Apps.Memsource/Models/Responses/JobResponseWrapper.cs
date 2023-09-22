using Apps.PhraseTMS.Models.Jobs.Responses;

namespace Apps.PhraseTMS.Models.Responses;

public class JobResponseWrapper
{
    public IEnumerable<CreateJobResponse> Jobs { get; set; }
}