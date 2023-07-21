using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Translations.Requests
{
    public class TranslateMTRequest
    {

        [JsonProperty("from")]
        [Display("Sourcce language code")]
        public string SourceLanguageCode { get; set; }

        [JsonProperty("to")]
        [Display("Target language code")]
        public string TargetLanguageCode { get; set; }

        [Display("Source texts")]
        public IEnumerable<string> SourceTexts { get; set; }
        
        [Display("File name")]
        public string? Filename { get; set; }
    }
}
