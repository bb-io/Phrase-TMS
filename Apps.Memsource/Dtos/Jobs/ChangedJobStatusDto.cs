using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Jobs;

public class ChangedJobStatusDto
{
    [JsonProperty("changedBy")]
    public MinimalUserDto ChangedBy { get; set; } = null!;
    
    [JsonProperty("changedDate")]
    public DateTime ChangedDate { get; set; }
    
    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;
}