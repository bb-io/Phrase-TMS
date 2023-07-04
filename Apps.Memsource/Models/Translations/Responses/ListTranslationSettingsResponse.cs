using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Translations.Responses
{
    public class ListTranslationSettingsResponse
    {
        public IEnumerable<TranslationSettingDto> TranslationSettings { get; set; }
    }
}
