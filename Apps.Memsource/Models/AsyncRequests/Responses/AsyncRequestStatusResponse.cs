using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.AsyncRequests.Responses;

public class AsyncRequestStatusResponse
{
    [Display("Async request ID")]
    public string AsyncRequestId { get; set; } = string.Empty;

    [Display("Action")]
    public string Action { get; set; } = string.Empty;

    [Display("Requested at")]
    public string RequestedAt { get; set; } = string.Empty;

    [Display("Finished")]
    public bool Finished { get; set; }

    [Display("Finished at")]
    public string? FinishedAt { get; set; }

    [Display("Error code")]
    public string? ErrorCode { get; set; }

    [Display("Error description")]
    public string? ErrorDescription { get; set; }
}
