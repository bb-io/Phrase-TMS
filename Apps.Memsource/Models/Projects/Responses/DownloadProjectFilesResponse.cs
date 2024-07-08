using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Projects.Responses;

public class DownloadProjectFilesResponse
{
    public List<FileReference> Files { get; set; } = new();
}