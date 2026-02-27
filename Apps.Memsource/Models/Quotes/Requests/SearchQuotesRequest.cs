using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Quotes.Requests;

public class SearchQuotesRequest
{
    public string? Name { get; set; }

    [Display("Quote status")]
    [StaticDataSource(typeof(QuoteStatusDataHandler))]
    public string? Status { get; set; }
}
