using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Connectors.Requests;

public class GetConnectorRequest
{
    [Display("Connector UID")]
    public string ConnectorUId { get; set; }
}