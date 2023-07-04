namespace Apps.PhraseTMS.Dtos
{
    public class QuoteDto
    {
        public bool Editable { get; set; }
        public string Currency { get; set; }
        public DateTime DateCreated { get; set; }
        public int Id { get; set; }
        public string BillingUnit { get; set; }
        public double TotalPrice { get; set; }
        public PriceListDto PriceList { get; set; }
        public bool Outdated { get; set; }
        public string Name { get; set; }
        public object NetRateScheme { get; set; }
        public string Uid { get; set; }
        public string Status { get; set; }
        public string QuoteType { get; set; }
    }
}
