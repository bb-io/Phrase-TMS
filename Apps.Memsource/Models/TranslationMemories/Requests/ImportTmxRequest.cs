using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class ImportTmxRequest
    {
        [Display("Translation memory")]
        [DataSource(typeof(TmDataHandler))]
        public string TranslationMemoryUId { get; set; }

        public byte[] File { get; set; }

        [Display("File name")]
        public string FileName { get; set; }
    }
}
