using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests
{
    public class ListReferenceFilesRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }
    }
}