using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class DownloadLqaResponse
{
    [Display("LQA report")]
    public FileReference LqaReport { get; set; } = new();
}