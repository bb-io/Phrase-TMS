namespace Apps.PhraseTMS.Models.Translations.Requests
{
    public class TranslateMTRequest
    {
        public string MTSettingsUId { get; set; }

        public string SourceLanguageCode { get; set; }

        public string TargetLanguageCode { get; set; }

        public IEnumerable<string> SourceTexts { get; set; }
    }
}
