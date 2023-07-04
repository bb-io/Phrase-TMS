namespace Apps.PhraseTMS.Models.TranslationMemories.Requests
{
    public class CreateTranslationMemoryRequest
    {
        public string Name { get; set; }

        public string SourceLang { get; set; }

        public string TargetLang { get; set; }
    }
}
