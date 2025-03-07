using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class UpdateTargetFileInput
{
    [JsonIgnore]
    public FileReference File { get; set; }

    [Display("Propagate confirmed to TM")]
    public bool? PropagateConfirmedToTm { get; set; }
    
    [Display("Unconfirm changed segments")]
    public bool? UnconfirmChangedSegments { get; set; }
}