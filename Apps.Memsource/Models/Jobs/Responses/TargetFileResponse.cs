using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class TargetFileResponse
    {

        [Display("File")]
        public File File { get; set; }
    }
}
