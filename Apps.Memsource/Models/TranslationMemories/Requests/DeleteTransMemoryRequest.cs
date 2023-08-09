using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class DeleteTransMemoryRequest
    {
        [Display("Translation memory")] 
        [DataSource(typeof(TmDataHandler))]
        public string TranslationMemoryUId { get; set; }

        public bool? Purge { get; set; }
    }
}