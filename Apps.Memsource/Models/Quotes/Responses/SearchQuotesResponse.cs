using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Quotes.Responses;

public class SearchQuotesResponse
{
    public IEnumerable<QuoteDto> Quotes { get; set; } = new List<QuoteDto>();
}
