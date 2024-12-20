using Apps.PhraseTMS.Models.Jobs.Responses;

namespace Apps.PhraseTMS.Webhooks.Handlers.Models;

public class JobsResponse
{
    public List<JobResponse> Jobs { get; set; } = new();
}