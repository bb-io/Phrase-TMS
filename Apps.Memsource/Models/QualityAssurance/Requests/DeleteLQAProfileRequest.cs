using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests
{
    public class DeleteLQAProfileRequest
    {
        [Display("LQA profile UID")]
        public string LQAProfileUId { get; set; }
    }
}
