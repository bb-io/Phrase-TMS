using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;

public class DownloadReferenceFilesRequest : ProjectRequest
{
    [Display("Reference file UID")] public string ReferenceFileUId { get; set; }
}