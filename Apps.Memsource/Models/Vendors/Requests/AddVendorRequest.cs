using System.Collections;
using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Vendors.Requests
{
    public class AddVendorRequest
    {
        [Display("Vendor token")]
        public string VendorToken { get; set; }

        [Display("Price list UID")]
        public string PriceListUId { get; set; }
        
        [Display("Source locales")]
        public IEnumerable<string>? SourceLocales { get; set; }

        [Display("Target locales")]
        public IEnumerable<string>? TargetLocales { get; set; }
    }
}
