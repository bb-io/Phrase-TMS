using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;

public class ReferenceFileRequest
{
    [Display("Reference file UID")] 
    public string ReferenceFileUId { get; set; }
}