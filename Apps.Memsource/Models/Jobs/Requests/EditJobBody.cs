using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class EditJobBody
{
    [Display("Status"), StaticDataSource(typeof(UpdateJobStatusDataHandler))]
    public string? Status { get; set; }

    [Display("Notify owner", 
        Description = "Notifies project owner and job owner about the status change. Only used when Status is set. Default is false")]
    public bool? NotifyOwnerAboutStatus { get; set; }

    [Display("Propagate status", 
        Description = "Updates the status and notifies previous/next step providers. Only used when Status is set. Default is false")]
    public bool? PropagateStatus { get; set; }
    
    [Display("Due date")]
    public DateTime? DateDue { get; set; }
}