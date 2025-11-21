using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

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
    [JsonProperty("uid")]
    [Display("Project ID")]
    public string ProjectId { get; set; }

    [Display("Project name")]
    public string Name { get; set; }
}