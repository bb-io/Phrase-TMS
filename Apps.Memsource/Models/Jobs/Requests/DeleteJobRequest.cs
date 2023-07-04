namespace Apps.PhraseTms.Models.Jobs.Requests
{
    public class DeleteJobRequest
    {
        public string ProjectUId { get; set; }

        public IEnumerable<string> JobsUIds { get; set; }
    }
}
