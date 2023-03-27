using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource
{
    public class MemsourceRequest : RestRequest
    {
        public MemsourceRequest(string endpoint, Method method, string token) : base(endpoint, method)
        {
            this.AddHeader("Authorization", token);
            this.AddHeader("accept", "*/*");
        }
    }
}
