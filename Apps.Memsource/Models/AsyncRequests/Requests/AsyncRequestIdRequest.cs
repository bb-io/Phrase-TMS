using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.AsyncRequests.Requests;

public class AsyncRequestIdRequest
{
    [Display("Async request ID")]
    public string AsyncRequestId { get; set; } = string.Empty;
}
