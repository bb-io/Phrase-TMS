namespace Apps.PhraseTms.Models.Projects.Requests
{
    public class EditProjectRequest
    {
        public string ProjectUId { get; set; }

        public string ProjectName { get; set; }

        public string Status { get; set; } //"ACCEPTED_BY_VENDOR" "ASSIGNED" "CANCELLED" "COMPLETED" "COMPLETED_BY_VENDOR" "DECLINED_BY_VENDOR" "NEW"
    }
}
