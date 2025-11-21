using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Apps.PhraseTMS.Dtos.Analysis;

public class AnalysisDto
{
    [JsonProperty("uid")]
    [Display("Analysis ID")]
    public string UId { get; set; }

    public string Name { get; set; }

    public SimpleProjectDto Project { get; set; }
}

public class SimpleProjectDto
{
    [JsonPropertyName("uid")]
    [Display("Project ID")]
    public string ProjectId { get; set; }

    [Display("Project name")]
    public string Name { get; set; }
}