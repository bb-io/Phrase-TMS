using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;

public class DeleteReferenceFileRequest
{
    [Display("Reference file UID")] 
    public string ReferenceFileUId { get; set; }
}