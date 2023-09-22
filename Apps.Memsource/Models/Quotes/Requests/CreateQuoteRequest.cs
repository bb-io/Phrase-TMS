using Apps.PhraseTMS.DataSourceHandlers;
using Apps.PhraseTMS.Models.Projects.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Quotes.Requests;

public class CreateQuoteRequest : ProjectRequest
{
    [Display("Analyse UID")] public string AnalyseUId { get; set; }
    public string Name { get; set; }
        
    [Display("Price list")] 
    [DataSource(typeof(PriceListDataHandler))]
    public string PriceListUId { get; set; }
}