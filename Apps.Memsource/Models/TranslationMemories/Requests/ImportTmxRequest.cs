using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class ImportTmxRequest
    {
        [Display("Translation memory")]
        [DataSource(typeof(TmDataHandler))]
        public string TranslationMemoryUId { get; set; }

        [Display("File")]
        public File File { get; set; }
    }
}
