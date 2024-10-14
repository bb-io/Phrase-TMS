using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class ClientDto
{
    public string Name { get; set; }

    [Display("Client ID")]
    public string UId { get; set; }
        
    [Display("External ID")]

    public string ExternalId { get; set; }
}