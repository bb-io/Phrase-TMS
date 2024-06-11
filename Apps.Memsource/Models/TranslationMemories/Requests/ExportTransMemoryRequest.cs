using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.TranslationMemories.Requests;

public class ExportTransMemoryRequest
{
    [Display("Translation memory UID")]
    [DataSource(typeof(TmDataHandler))]
    public string TranslationMemoryUId { get; set; }

    [Display("File format")]
    [StaticDataSource(typeof(TmFormatDataHandler))]
    public string FileFormat { get; set; }
}