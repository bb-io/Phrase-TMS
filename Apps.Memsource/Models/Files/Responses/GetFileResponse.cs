using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;


namespace Apps.PhraseTMS.Models.Files.Responses;

public class GetFileResponse
{
    [Display("File")]
    public FileReference File { get; set; }
}