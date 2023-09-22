using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Responses;

public class ConnectorsResponseWrapper
{
    public IEnumerable<ConnectorDto> Connectors { get; set; }
}