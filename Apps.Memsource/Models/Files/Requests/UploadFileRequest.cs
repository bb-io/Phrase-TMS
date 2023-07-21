using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Files.Requests
{
    public class UploadFileRequest
    {
        [JsonIgnore]
        public byte[] File { get; set; }

        [JsonIgnore]
        [Display("File name")]
        public string FileName { get; set; }
        
        public string? Url { get; set; }
    }
}
