using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class ExportTransMemoryBody
{
    [Display("Export Target Languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string>? ExportTargetLangs { get; set; }
}