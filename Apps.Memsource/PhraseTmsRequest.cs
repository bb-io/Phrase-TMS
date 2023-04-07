using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTms
{
    public class PhraseTmsRequest : RestRequest
    {
        public PhraseTmsRequest(string endpoint, Method method, string token) : base(endpoint, method)
        {
            this.AddHeader("Authorization", $"ApiToken {token}");
            this.AddHeader("accept", "*/*");
        }
    }
}
