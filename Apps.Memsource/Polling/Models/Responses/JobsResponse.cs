using Apps.PhraseTMS.Models.Jobs.Responses;

namespace Apps.PhraseTMS.Polling.Models.Responses;

public class JobsResponse
{
    public List<JobResponse> Jobs { get; set; } = new();
}