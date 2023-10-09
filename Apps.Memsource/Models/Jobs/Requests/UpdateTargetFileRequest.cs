namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class UpdateTargetFileRequest
{
    public IEnumerable<UidRequest> Jobs { get; set; }
    
    public bool PropagateConfirmedToTm { get; set; }
    
    public bool UnconfirmChangedSegments { get; set; }
}