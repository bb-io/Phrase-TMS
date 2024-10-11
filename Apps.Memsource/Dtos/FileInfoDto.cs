using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class FileInfoDto
{
    public string Name { get; set; }

    [Display("File ID")]
    public string UId { get; set; }

    public int Size { get; set; }

    public string Type { get; set; }
}