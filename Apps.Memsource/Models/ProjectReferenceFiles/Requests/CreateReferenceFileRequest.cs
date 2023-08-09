using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests
{
    public class CreateReferenceFileRequest : ProjectRequest
    {
        public byte[] File { get; set; }

        [Display("File name")]
        public string FileName { get; set; }
    }
}