using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Responses
{
    public class DownloadReferenceFilesResponse
    {
        public byte[] File { get; set; }

        [Display("File name")]
        public string Filename { get; set; }
    }
}
