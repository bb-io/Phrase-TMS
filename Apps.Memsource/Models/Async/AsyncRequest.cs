using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Async;

public class AsyncRequestResponse 
{
    public AsyncRequest AsyncRequest { get; set; }
}

public class AsyncResponse
{
    [Display("Creation date")]
    public string DateCreated { get; set; } = string.Empty;

    [Display("Error code"), JsonProperty("errorCode")]
    public string? ErrorCode { get; set; }

    [Display("Error description"), JsonProperty("errorDesc")]
    public string? ErrorDescription { get; set; }
}

public class AsyncRequest
{
    [Display("ID")]
    public string Id { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;
        
    [Display("Creation date")]
    public string DateCreated { get; set; } = string.Empty;
    
    [Display("Async response")]
    public AsyncResponse? AsyncResponse { get; set; }

}