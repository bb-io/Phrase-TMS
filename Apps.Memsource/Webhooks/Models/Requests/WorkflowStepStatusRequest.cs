using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Webhooks.Models.Requests;

public class WorkflowStepStatusRequest : ProjectRequest
{
    [Display("Workflow step ID"), DataSource(typeof(WorkflowStepDataHandler))]
    public string WorkflowStepId { get; set; } = string.Empty;

    [Display("Job status"), StaticDataSource(typeof(JobStatusDataHandler))]
    public string JobStatus { get; set; } = string.Empty;
}