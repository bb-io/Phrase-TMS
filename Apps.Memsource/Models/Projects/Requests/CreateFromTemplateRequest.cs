using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class CreateFromTemplateRequest
{
    public string Name { get; set; }

    [Display("Template UID")]
    [DataSource(typeof(ProjectTemplateDataHandler))]
    public string TemplateUId { get; set; }
    
    [Display("Due date")]
    public DateTime? DateDue { get; set; }
}