using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;


namespace Apps.PhraseTMS.Models.Jobs.Responses;

public class TargetFileResponse
{

    [Display("File")]
    public FileReference File { get; set; }
}