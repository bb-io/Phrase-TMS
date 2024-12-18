using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class UpdateJobTargetFileRequest : JobRequest
{
    public FileReference File { get; set; } = default!;

    [Display("Propagate confirmed to TM", Description = "By default this value set to true")]
    public bool? PropagateConfirmedToTm { get; set; }
}