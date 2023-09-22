using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.PhraseTMS.Models.Files.Requests;

public class UploadFileRequest
{
    [JsonIgnore]
    public File File { get; set; }

    [JsonIgnore]
    [Display("File name")]
    public string? FileName { get; set; }
}