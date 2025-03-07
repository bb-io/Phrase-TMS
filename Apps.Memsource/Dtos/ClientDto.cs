using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class ClientDto
{
    public string Name { get; set; }

    [Display("Client ID")]
    public string UId { get; set; }
        
    [Display("External ID")]

    public string ExternalId { get; set; }

    public string? Note { get; set; }

    [Display("Display note in property?")]
    [JsonProperty("displayNoteInProject")]
    public bool DisplayNoteInProject { get; set; }
}