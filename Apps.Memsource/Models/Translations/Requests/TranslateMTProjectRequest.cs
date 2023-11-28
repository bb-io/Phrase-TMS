using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Translations.Requests;

public class TranslateMTProjectRequest : ProjectRequest
{
    [Display("Job")]
    [DataSource(typeof(JobDataHandler))]
    public string JobUId { get; set; }

    [Display("Source texts")] 
    public IEnumerable<string> SourceTexts { get; set; }
}