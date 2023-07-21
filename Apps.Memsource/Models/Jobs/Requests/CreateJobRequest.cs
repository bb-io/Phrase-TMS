using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class CreateJobRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }
        [Display("Target languages")] public IEnumerable<string> TargetLanguages { get; set; }
        public byte[] File { get; set; }
        [Display("File name")] public string FileName { get; set; }
        [Display("File type")] public string FileType { get; set; }
    }
}