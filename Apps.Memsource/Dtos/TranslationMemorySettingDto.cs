using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class TranslationMemorySettingDto
{
    [JsonProperty("confirm100PercentMatches")]
    public bool Confirm100PercentMatches { get; set; }
    
    [JsonProperty("confirm101PercentMatches")]
    public bool Confirm101PercentMatches { get; set; }
    
    [JsonProperty("lock100PercentMatches")]
    public bool Lock100PercentMatches { get; set; }
    
    [JsonProperty("lock101PercentMatches")]
    public bool Lock101PercentMatches { get; set; }

    [JsonProperty("translationMemoryThreshold")]
    public double TranslationMemoryThreshold { get; set; }

    [JsonProperty("useTranslationMemory")]
    public bool UseTranslationMemory { get; set; }
}