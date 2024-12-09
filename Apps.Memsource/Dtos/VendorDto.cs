using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class VendorDto
{
    [Display("Vendor ID")]
    public string UId { get; set; }

    public string Name { get; set; }

    [Display("Vendor token")]
    public string VendorToken { get; set; }

    [Display("Short ID")] public string Id { get; set; }
}