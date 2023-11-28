using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class GetAnalysisRequest : GetJobRequest
{
    [Display("Analysis")]
    [DataSource(typeof(AnalysisDataHandler))]
    public string AnalysisUId { get; set; }
}