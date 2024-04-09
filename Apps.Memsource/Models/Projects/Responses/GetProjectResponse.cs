using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Responses;

public class GetProjectResponse
{
    public string Name { get; set; }

    [Display("ID")]
    public string Id { get; set; }

    [Display("Creation date")]
    public DateTime DateCreated { get; set; }

    [Display("UID")]
    public string UId { get; set; }

    [Display("Source Language")]
    public string SourceLanguage { get; set; }

    [Display("Target languages")]
    public List<string> TargetLangs { get; set; }

    [Display("Due date")]
    public DateTime? DateDue { get; set; }

    [Display("Status")]
    public string Status { get; set; }

    [Display("Note")]
    public string Note { get; set; }

    [Display("Client Name")]
    public string ClientName { get; set; }

    public string Domain { get; set; }

    public string SubDomain { get; set; }

    public string Owner { get; set; }
}