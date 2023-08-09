namespace Apps.PhraseTMS.Models.Projects.Requests
{
    public class DeleteProjectRequest : ProjectRequest
    {
        public bool? Purge { get; set; }
    }
}