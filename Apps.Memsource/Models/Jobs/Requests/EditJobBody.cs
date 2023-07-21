using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class EditJobBody
{
    public string Status { get; set; } //"ACCEPTED" "CANCELLED" "COMPLETED" "DECLINED" "DELIVERED" "EMAILED" "NEW" "REJECTED"

    [Display("Due date")]
    public string? DateDue { get; set; }
}