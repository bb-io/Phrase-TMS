using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Responses
{
    public class TargetFileResponse
    {
        [Display("File name")]
        public string Filename { get; set; }

        public byte[] File { get; set; }
    }
}
