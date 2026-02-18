using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class WorkflowStepStatusRequest : ProjectWebhookRequest
{
    [Display("Workflow step ID")]
    [DataSource(typeof(WorkflowStepDataHandler))]
    public string WorkflowStepId { get; set; } = string.Empty;

    [Display("Job statuses", Description = "Start an event if all jobs are in any of the selected statuses.")]
    [StaticDataSource(typeof(JobWebhookStatusDataHandler))]
    public IEnumerable<string> JobStatuses { get; set; } = [];
}