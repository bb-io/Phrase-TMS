using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Quotes.Requests;

public class CreateQuoteRequest
{
    [Display("Analyse")]
    [DataSource(typeof(AnalysisDataHandler))]
    public string AnalyseUId { get; set; }
    public string Name { get; set; }
        
    [Display("Price list")] 
    [DataSource(typeof(PriceListDataHandler))]
    public string PriceListUId { get; set; }
}