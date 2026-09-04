using Apps.PhraseTMS.Dtos.Jobs;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Responses;

public class JobStatusChangeResponse(ChangedJobStatusDto dto)
{
    [Display("Changed by user UID")]
    public string ChangedByUid { get; set; } = dto.ChangedBy.UId;

    [Display("Changed by user full name")]
    public string ChangedByUserName { get; set; } = $"{dto.ChangedBy.FirstName} {dto.ChangedBy.LastName}";

    [Display("Changed by user email")]
    public string ChangedByUserEmail { get; set; } = dto.ChangedBy.Email;

    [Display("Changed by user role")]
    public string ChangedByUserRole { get; set; } = dto.ChangedBy.Role;

    [Display("Changed date")]
    public DateTime ChangedDate { get; set; } = dto.ChangedDate;
    
    [Display("Status")]
    public string Status { get; set; } = dto.Status;
}