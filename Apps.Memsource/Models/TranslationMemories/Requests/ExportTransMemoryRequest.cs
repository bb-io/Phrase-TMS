using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class ExportTransMemoryRequest
    {
        [Display("Translation memory")]
        [DataSource(typeof(TmDataHandler))]
        public string TranslationMemoryUId { get; set; }

        [Display("File format")]
        [DataSource(typeof(TmFormatDataHandler))]
        public string FileFormat { get; set; }
    }
}
