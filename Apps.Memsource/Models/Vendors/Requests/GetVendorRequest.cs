using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Models.Vendors.Requests
{
    public class GetVendorRequest
    {
        [Display("Vendor ID")]
        public string VendorId { get; set; }
    }
}
