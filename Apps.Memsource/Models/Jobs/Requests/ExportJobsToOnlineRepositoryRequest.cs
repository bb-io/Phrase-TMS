namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class ExportJobsToOnlineRepositoryRequest
    {
        public IEnumerable<string> JobIds { get; set; }
    }
}
