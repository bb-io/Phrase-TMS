using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class ProjectDto
{
    public string Name { get; set; }

    //public string Id { get; set; }

    [Display("UID")]
    public string UId { get; set; }

    [Display("Creation date")]
    public DateTime DateCreated { get; set; }
}