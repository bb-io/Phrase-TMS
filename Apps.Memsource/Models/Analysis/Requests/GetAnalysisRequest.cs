using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class GetAnalysisRequest
{
    [Display("Analysis ID")]
    public string AnalysisUId { get; set; }
}