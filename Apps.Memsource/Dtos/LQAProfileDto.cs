using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class LqaProfileDto
{
    [Display("UID")]
    public string Uid { get; set; }

    public string Name { get; set; }
}