using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Clients.Requests;

public class ListClientsQuery
{
    [Display("Name")]
    public string? Name { get; set; }

    [Display("Created by")]
    public string? CreatedBy { get; set; }
}