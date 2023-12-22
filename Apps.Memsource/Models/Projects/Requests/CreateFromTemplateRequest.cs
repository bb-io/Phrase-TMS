using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class CreateFromTemplateRequest
{
    public string Name { get; set; }

    [Display("Template")]
    [DataSource(typeof(ProjectTemplateDataHandler))]
    public string TemplateUId { get; set; }
    
    [Display("Date due")]
    public DateTime? DateDue { get; set; }
}