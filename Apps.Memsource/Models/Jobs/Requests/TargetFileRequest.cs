using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class TargetFileRequest
{
    [Display("Job")]
    [DataSource(typeof(JobDataHandler))]
    public string JobUId { get; set; }
}