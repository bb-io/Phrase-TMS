using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Translations.Requests;

public class TranslateMtRequest
{
    [JsonProperty("from")]
    [Display("Source language code")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLanguageCode { get; set; }

    [JsonProperty("to")]
    [Display("Target language code")]
    [DataSource(typeof(LanguageDataHandler))]
    public string TargetLanguageCode { get; set; }

    [Display("Text")]
    public string Text { get; set; }

    [Display("Source texts")]
    [DefinitionIgnore]
    public IEnumerable<string> SourceTexts => new List<string> { Text };
        
    [Display("File name")]
    public string? Filename { get; set; }
}