using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Connectors.Requests;

public class GetConnectorRequest
{
    [Display("Connector UID")]
    [DataSource(typeof(ConnectorDataHandler))]
    public string ConnectorUId { get; set; }
}