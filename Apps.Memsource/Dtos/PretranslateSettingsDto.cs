using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class PretranslateSettingsDto
{
    [JsonProperty("translationMemorySettings")]
    public TranslationMemorySettingDto TmSettings { get; set; } = null!;
}