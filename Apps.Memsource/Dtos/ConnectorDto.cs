using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class ConnectorDto
{
    public string Name { get; set; }

    public string Type { get; set; }

    [Display("ID")]
    public string Id { get; set; }
}