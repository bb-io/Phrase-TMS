using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.PhraseTMS.Models.Files.Responses;

public class GetFileResponse
{
    [Display("File")]
    public File File { get; set; }
}