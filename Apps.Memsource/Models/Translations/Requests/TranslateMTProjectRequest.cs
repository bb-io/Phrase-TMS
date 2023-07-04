namespace Apps.PhraseTMS.Models.Translations.Requests
{
    public class TranslateMTProjectRequest
    {
        public string ProjectUId { get; set; }

        public string JobUId { get; set; }

        public IEnumerable<string> SourceTexts { get; set; }
    }
}
