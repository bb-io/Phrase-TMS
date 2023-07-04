namespace Apps.PhraseTMS.Dtos
{
    public class TranslationMemoryDto
    {
        public string UId { get; set; }

        public string Name { get; set; }

        public string SourceLang { get; set; }

        public IEnumerable<string> TargetLangs { get; set; }
    }
}
