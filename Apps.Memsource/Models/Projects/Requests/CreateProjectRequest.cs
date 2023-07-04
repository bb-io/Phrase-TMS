namespace Apps.PhraseTms.Models.Projects.Requests
{
    public class CreateProjectRequest
    {
        public string Name { get; set; }

        public string SourceLanguage { get; set; }

        public IEnumerable<string> TargetLanguages { get; set; }
    }
}
