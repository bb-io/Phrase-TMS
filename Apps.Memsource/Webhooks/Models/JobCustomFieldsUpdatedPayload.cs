using Newtonsoft.Json;

namespace Apps.PhraseTMS.Webhooks.Models;

public class JobCustomFieldsUpdatedPayload
{
    [JsonProperty("jobParts")]
    public List<JobCustomFieldsUpdatedJobPart> JobParts { get; set; } = [];

    [JsonProperty("metadata")]
    public ProjectMetadata? Metadata { get; set; }

    [JsonProperty("timestamp")]
    public long Timestamp { get; set; }

    [JsonProperty("eventUid")]
    public string? EventUId { get; set; }
}

public class JobCustomFieldsUpdatedJobPart
{
    [JsonProperty("uid")]
    public string UId { get; set; } = string.Empty;

    [JsonProperty("project")]
    public Project? Project { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("fileName")]
    public string? FileName { get; set; }

    [JsonProperty("targetLang")]
    public string? TargetLang { get; set; }

    [JsonProperty("workflowLevel")]
    public int WorkflowLevel { get; set; }

    [JsonProperty("customFields")]
    public List<JobCustomFieldPayload> CustomFields { get; set; } = [];
}

public class JobCustomFieldPayload
{
    [JsonProperty("uid")]
    public string UId { get; set; } = string.Empty;

    [JsonProperty("customField")]
    public JobCustomFieldDefinitionPayload? CustomField { get; set; }

    [JsonProperty("selectedOptions")]
    public List<JobCustomFieldOptionPayload> SelectedOptions { get; set; } = [];

    [JsonProperty("value")]
    public string? Value { get; set; }

    [JsonProperty("createdAt")]
    public DateTimeOffset? CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTimeOffset? UpdatedAt { get; set; }
}

public class JobCustomFieldDefinitionPayload
{
    [JsonProperty("uid")]
    public string UId { get; set; } = string.Empty;

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}

public class JobCustomFieldOptionPayload
{
    [JsonProperty("uid")]
    public string UId { get; set; } = string.Empty;

    [JsonProperty("value")]
    public string? Value { get; set; }
}
