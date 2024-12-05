using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class EditJobBody
{
    [StaticDataSource(typeof(JobStatusDataHandler))]
    public string? Status { get; set; } 
    
    [Display("Due date")]
    public DateTime? DateDue { get; set; }
}