using Newtonsoft.Json;

namespace Apps.PhraseTMS.Dtos;

public class CreateReferenceFilesDto
{
    [JsonProperty("referenceFiles")] 
    public List<ReferenceFileInfoDto> ReferenceFiles { get; set; } = new();
}