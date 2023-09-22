using Apps.PhraseTMS.Models.Projects.Requests;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.PhraseTMS.Models.ProjectReferenceFiles.Requests;

public class CreateReferenceFileRequest : ProjectRequest
{
    public File File { get; set; }
}