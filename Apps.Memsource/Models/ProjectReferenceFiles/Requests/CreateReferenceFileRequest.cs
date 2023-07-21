using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests
{
    public class CreateReferenceFileRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }

        public byte[] File { get; set; }

        [Display("File name")]
        public string FileName { get; set; }
    }
}