using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Vendors.Requests;

public class GetVendorRequest
{
    [Display("Vendor ID")]
    [DataSource(typeof(VendorDataHandler))]
    public string VendorId { get; set; }
}