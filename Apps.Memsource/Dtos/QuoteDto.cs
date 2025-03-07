using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class QuoteDto
{
    [Display("Quote ID")] public string Uid { get; set; }
    public string Status { get; set; }
    [Display("Quote type")] public string QuoteType { get; set; }
    public bool Editable { get; set; }
    public string Currency { get; set; }
    [Display("Created date")] public DateTime DateCreated { get; set; }
    [Display("Quote ID")] public int Id { get; set; }
    [Display("Billing unit")] public string BillingUnit { get; set; }
    [Display("Total price")] public double TotalPrice { get; set; }
    [Display("Price list")] public PriceListDto PriceList { get; set; }
    public bool Outdated { get; set; }
    public string Name { get; set; }
    [Display("Net rate scheme")] public object NetRateScheme { get; set; }

}