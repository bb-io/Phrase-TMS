using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;


namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;

public class CreateReferenceFileRequest
{
    [Display("Reference files")]
    public IEnumerable<FileReference>? Files { get; set; }
    
    [Display("Note")]
    public string? Note { get; set; }
}