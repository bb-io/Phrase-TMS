namespace Apps.PhraseTms.Models.Projects.Requests
{
    public class AddTargetLanguageRequest
    {
        public string ProjectUId { get; set; }

        public IEnumerable<string> TargetLanguages { get; set; }
    }
}
