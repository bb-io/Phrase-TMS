using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Files.Requests
{
    public class GetFileRequest
    {
        [Display("File UID")]
        public string FileUId { get; set; }
    }
}
