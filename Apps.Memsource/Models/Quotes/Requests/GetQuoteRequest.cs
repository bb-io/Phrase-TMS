using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Quotes.Requests;

public class GetQuoteRequest
{
    [Display("Quote UID")]
    public string QuoteUId { get; set; }
}