﻿namespace Apps.PhraseTMS.Models.Quotes.Requests
{
    public class CreateQuoteRequest
    {
        public string AnalyseUId { get; set; }

        public string Name { get; set; }

        public string PriceListUId { get; set; }

        public string ProjectUId { get; set; }
    }
}
