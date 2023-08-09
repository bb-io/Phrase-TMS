using Apps.PhraseTMS.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.PriceLists.Response;

public class ListPriceListsResponse
{
    [Display("Price lists")]
    public List<PriceListDto> PriceLists { get; set; }
}