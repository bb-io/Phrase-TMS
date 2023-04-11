using Apps.PhraseTMS.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Vendors.Response
{
    public class ListVendorsResponse
    {
        public IEnumerable<VendorDto> Vendors { get; set; } 
    }
}
