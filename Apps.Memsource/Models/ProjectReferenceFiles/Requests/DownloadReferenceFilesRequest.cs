using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests
{
    public class DownloadReferenceFilesRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }
        [Display("Reference file UID")] public string ReferenceFileUId { get; set; }
    }
}