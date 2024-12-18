using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Polling.Models.Requests;

public class OnAllJobsInWorkflowStepReachedStatusRequest : ProjectRequest
{
    [Display("Workflow step UID"), DataSource(typeof(WorkflowStepDataHandler))]
    public string WorkflowStepUid { get; set; } = string.Empty;

    [Display("Job status"), StaticDataSource(typeof(JobStatusDataHandler))]
    public string JobStatus { get; set; } = string.Empty;
}