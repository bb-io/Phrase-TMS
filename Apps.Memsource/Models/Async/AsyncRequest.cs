using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Async;

public class AsyncRequestResponse 
{
    public AsyncRequest AsyncRequest { get; set; }
}

public class AsyncResponse
{
    [Display("Creation date")]
    public string DateCreated { get; set; }
}

public class AsyncRequest
{
    [Display("ID")]
    public string Id { get; set; }
    public string Action { get; set; }
        
    [Display("Creation date")]
    public string DateCreated { get; set; }
    public AsyncResponse? AsyncResponse { get; set; }

}