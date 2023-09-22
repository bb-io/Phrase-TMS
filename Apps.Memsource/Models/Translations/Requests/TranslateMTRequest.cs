using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Translations.Requests;

public class TranslateMTRequest
{
    [JsonProperty("from")]
    [Display("Sourcce language code")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLanguageCode { get; set; }

    [JsonProperty("to")]
    [Display("Target language code")]
    [DataSource(typeof(LanguageDataHandler))]
    public string TargetLanguageCode { get; set; }

    [Display("Source texts")]
    public IEnumerable<string> SourceTexts { get; set; }
        
    [Display("File name")]
    public string? Filename { get; set; }
}