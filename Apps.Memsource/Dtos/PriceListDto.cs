
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos
{
    public class PriceListDto
    {
        [Display("Price list UID")] public string UId { get; set; }
        //public string Id { get; set; }
        [Display("Price list name")] public string Name { get; set; }
    }
}
