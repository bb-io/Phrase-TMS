namespace Apps.PhraseTms.Models.Jobs.Requests
{
    public class GetSegmentsRequest
    {
        public string ProjectUId { get; set; }

        public string JobUId { get; set; }

        public int BeginIndex { get; set; }

        public int EndIndex { get; set; }
    }
}
