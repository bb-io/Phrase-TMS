using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests
{
    public class DeleteLQAProfileRequest
    {
        [Display("LQA profile")]
        [DataSource(typeof(LQAProfileDataHandler))]
        public string LQAProfileUId { get; set; }
    }
}
