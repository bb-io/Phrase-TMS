using Apps.PhraseTMS.Dtos;

namespace Apps.PhraseTMS.Models.Vendors.Response;

public class ListVendorsResponse
{
    public IEnumerable<VendorDto> Vendors { get; set; } 
}