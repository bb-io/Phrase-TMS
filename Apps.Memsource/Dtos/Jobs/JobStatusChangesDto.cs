using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos.Jobs;

public class JobStatusChangesDto
{
    [JsonProperty("statusChanges")]
    public List<ChangedJobStatusDto> StatusChanges { get; set; } = [];
}