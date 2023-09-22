using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class LQAProfileDto
{
    [Display("UID")]
    public string Uid { get; set; }

    public string Name { get; set; }
}