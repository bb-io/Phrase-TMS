using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Translations.Responses
{
    public class ListTranslationSettingsResponse
    {
        [Display("Translation settings")]
        public IEnumerable<TranslationSettingDto> TranslationSettings { get; set; }
    }
}
