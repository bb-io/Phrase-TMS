using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class UploadBilingualFileRequest
    {
        public FileReference File { get; set; }

        [StaticDataSource(typeof(SaveToTmDataHandler))]
        [Display("Save to translation memory?")]
        public string? saveToTransMemory { get; set; }

        [Display("Set as completed?")]
        public bool? setCompleted { get; set; }
    }
}
