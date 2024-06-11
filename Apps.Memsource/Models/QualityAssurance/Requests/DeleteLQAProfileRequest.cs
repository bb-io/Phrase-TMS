using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests;

public class DeleteLqaProfileRequest
{
    [Display("LQA profile UID")]
    [DataSource(typeof(LqaProfileDataHandler))]
    public string LqaProfileUId { get; set; }
}