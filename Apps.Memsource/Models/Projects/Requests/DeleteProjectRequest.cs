using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class DeleteProjectRequest
    {
        [Display("Project UID")] public string ProjectUId { get; set; }
        public bool? Purge { get; set; }
    }
}