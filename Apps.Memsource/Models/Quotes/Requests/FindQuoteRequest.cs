using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.Models.Quotes.Requests;

public class FindQuoteRequest
{
    [Display("Project ID")]
    public string ProjectUid { get; set; }

    public string? Name { get; set; }

    [Display("Quote status")]
    [StaticDataSource(typeof(QuoteStatusDataHandler))]
    public string? Status { get; set; }

    [Display("Quote type")]
    [StaticDataSource(typeof(QuoteTypeDataHandler))]
    public string? QuoteType { get; set; }

    [Display("Quote name contains")]
    public string? NameContains { get; set; }
}
