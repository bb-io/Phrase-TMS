using Apps.PhraseTMS.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class EditJobBody
{
    [DataSource(typeof(JobStatusDataHandler))]
    public string Status { get; set; } 
    
    [Display("Due date")]
    public string? DateDue { get; set; }
}