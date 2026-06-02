using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Webhooks.Models.Responses;

public class JobCustomFieldsUpdatedResponse
{
    [Display("Event UID")]
    public string? EventUId { get; set; }

    [Display("Event timestamp")]
    public string EventTimestamp { get; set; } = string.Empty;

    [Display("Jobs")]
    public IEnumerable<JobCustomFieldsUpdatedJobResponse> Jobs { get; set; } = [];
}

public class JobCustomFieldsUpdatedJobResponse
{
    [Display("Job ID")]
    public string JobUId { get; set; } = string.Empty;

    [Display("Project ID")]
    public string? ProjectUId { get; set; }

    [Display("Project name")]
    public string? ProjectName { get; set; }

    [Display("File name")]
    public string? FileName { get; set; }

    [Display("Target language code")]
    public string? TargetLang { get; set; }

    [Display("Workflow level")]
    public int WorkflowLevel { get; set; }

    [Display("Updated custom fields")]
    public IEnumerable<JobCustomFieldWebhookResponse> UpdatedCustomFields { get; set; } = [];
}

public class JobCustomFieldWebhookResponse
{
    [Display("Instance ID")]
    public string UId { get; set; } = string.Empty;

    [Display("Custom field ID")]
    public string? CustomFieldUId { get; set; }

    [Display("Custom field name")]
    public string? Name { get; set; }

    [Display("Custom field type")]
    public string? Type { get; set; }

    [Display("Value")]
    public string? Value { get; set; }

    [Display("Selected options")]
    public IEnumerable<JobCustomFieldOptionWebhookResponse> SelectedOptions { get; set; } = [];

    [Display("Created at")]
    public string? CreatedAt { get; set; }

    [Display("Updated at")]
    public string? UpdatedAt { get; set; }
}

public class JobCustomFieldOptionWebhookResponse
{
    [Display("Option ID")]
    public string UId { get; set; } = string.Empty;

    [Display("Value")]
    public string? Value { get; set; }
}
