using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class LqaProfileDto
{
    [Display("LQA profile ID")]
    public string Uid { get; set; }

    public string Name { get; set; }
}