using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Vendors.Requests
{
    public class AddVendorRequest
    {
        public string VendorToken { get; set; }

        public string PriceListUId { get; set; }
    }
}
