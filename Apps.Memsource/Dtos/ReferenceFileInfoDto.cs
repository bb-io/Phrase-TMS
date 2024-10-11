using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class ReferenceFileInfoDto
{
    [Display("File ID")]
    public string UId { get; set; } = string.Empty;
        
    [Display("File name")]
    public string Filename { get; set; } = string.Empty;
    
    public string Note { get; set; } = string.Empty;

    // public string Id { get; set; }
}