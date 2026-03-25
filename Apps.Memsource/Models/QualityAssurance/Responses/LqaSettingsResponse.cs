using Newtonsoft.Json;

namespace Apps.PhraseTMS.Models.QualityAssurance.Responses;

public class LqaSettingsResponse
{
    [JsonProperty("categories")]
    public List<LqaCategoryDto> Categories { get; set; } = new();

    [JsonProperty("severities")]
    public List<LqaSeverityDto> Severities { get; set; } = new();
}

public class LqaCategoryDto
{
    [JsonProperty("errorCategoryId")]
    public int ErrorCategoryId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("children")]
    public List<LqaCategoryDto>? Children { get; set; }
}

public class LqaSeverityDto
{
    [JsonProperty("severityId")]
    public int SeverityId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
