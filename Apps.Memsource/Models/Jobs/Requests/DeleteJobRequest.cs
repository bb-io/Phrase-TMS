using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Jobs.Requests;

public class DeleteJobRequest
{
    [Display("Jobs")]
    [DataSource(typeof(JobDataHandler))]
    public IEnumerable<string> JobsUIds { get; set; }
}