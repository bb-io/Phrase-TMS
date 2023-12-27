using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;


namespace Apps.PhraseTMS.Models.Files.Requests;

public class UploadFileRequest
{
    [JsonIgnore]
    public FileReference File { get; set; }

    [JsonIgnore]
    [Display("File name")]
    public string? FileName { get; set; }
}