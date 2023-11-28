using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Jobs.Requests;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Quotes.Requests;

public class CreateQuoteRequest : GetJobRequest
{
    [Display("Analyse")]
    [DataSource(typeof(AnalysisDataHandler))]
    public string AnalyseUId { get; set; }
    public string Name { get; set; }
        
    [Display("Price list")] 
    [DataSource(typeof(PriceListDataHandler))]
    public string PriceListUId { get; set; }
}