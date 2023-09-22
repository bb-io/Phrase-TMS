using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Vendors.Requests;

public class AddVendorRequest
{
    [Display("Vendor token")]
    public string VendorToken { get; set; }

    [Display("Price list")]
    [DataSource(typeof(PriceListDataHandler))]
    public string PriceListUId { get; set; }
        
    [Display("Source locales")]
    public IEnumerable<string>? SourceLocales { get; set; }

    [Display("Target locales")]
    public IEnumerable<string>? TargetLocales { get; set; }
}