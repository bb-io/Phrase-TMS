using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Clients.Requests;

public class ClientRequest
{
    [Display("Client ID")]
    [DataSource(typeof(ClientDataHandler))]
    public string ClientUid { get; set; }
}