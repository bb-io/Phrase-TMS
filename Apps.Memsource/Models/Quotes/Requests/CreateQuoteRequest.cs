using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Quotes.Requests
{
    public class CreateQuoteRequest
    {
        [Display("Analyse UID")] public string AnalyseUId { get; set; }
        public string Name { get; set; }
        [Display("Price list UID")] public string PriceListUId { get; set; }
        [Display("Project UID")] public string ProjectUId { get; set; }
    }
}