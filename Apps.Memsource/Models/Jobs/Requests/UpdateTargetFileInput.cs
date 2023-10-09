using Apps.PhraseTMS.Models.Files.Requests;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class UpdateTargetFileInput : UploadFileRequest
{
    [Display("Propagate confirmed to TM")]
    public bool? PropagateConfirmedToTm { get; set; }
    
    [Display("Unconfirm changed segments")]
    public bool? UnconfirmChangedSegments { get; set; }
}