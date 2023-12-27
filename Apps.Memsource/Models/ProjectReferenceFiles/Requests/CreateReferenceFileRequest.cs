using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common.Files;


namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;

public class CreateReferenceFileRequest
{
    public FileReference File { get; set; }
}