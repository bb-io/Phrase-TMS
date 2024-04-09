using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Translations.Requests;

public class TranslateMTProjectRequest
{
    [Display("Job")]
    [DataSource(typeof(JobDataHandler))]
    public string JobUId { get; set; }

    [Display("Source texts")] 
    public IEnumerable<string> SourceTexts { get; set; }
}