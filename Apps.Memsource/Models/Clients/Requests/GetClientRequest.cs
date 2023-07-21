using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Clients.Requests
{
    public class GetClientRequest
    {
        [Display("Client UID")]
        public string ClientUId { get; set; }
    }
}
