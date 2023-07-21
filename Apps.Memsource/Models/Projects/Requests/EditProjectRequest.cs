using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class EditProjectRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }

        [Display("Project name")] public string ProjectName { get; set; }

        public string Status { get; set; } //"ACCEPTED_BY_VENDOR" "ASSIGNED" "CANCELLED" "COMPLETED" "COMPLETED_BY_VENDOR" "DECLINED_BY_VENDOR" "NEW"
    }
}