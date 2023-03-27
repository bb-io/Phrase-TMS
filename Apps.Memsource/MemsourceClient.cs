using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource
{
    public class MemsourceClient : RestClient
    {
        public MemsourceClient(string url) : base(new RestClientOptions() { ThrowOnAnyError = true, BaseUrl = new Uri(url + "/web/api2/v1") }) { }
    }
}
