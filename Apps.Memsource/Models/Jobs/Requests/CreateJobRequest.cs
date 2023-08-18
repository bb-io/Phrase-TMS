using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class CreateJobRequest : ProjectRequest
    {
        [Display("Target languages")] public IEnumerable<string> TargetLanguages { get; set; }
        public byte[] File { get; set; }
        
        [Display("File name")] 
        public string FileName { get; set; }
        [Display("File type")] public string FileType { get; set; }
    }
}