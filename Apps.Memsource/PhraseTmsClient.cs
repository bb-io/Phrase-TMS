using Apps.PhraseTMS.Models.Async;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTms
{
    public class PhraseTmsClient : RestClient
    {
        public PhraseTmsClient(string url) : base(new RestClientOptions() { ThrowOnAnyError = true, BaseUrl = new Uri(url + "/web") }) { }

        public AsyncRequest? PerformAsyncRequest(PhraseTmsRequest request, AuthenticationCredentialsProvider provider)
        {
            var asyncRequestResponse = this.Execute<AsyncRequestResponse>(request).Data;
            if (asyncRequestResponse is null) return default;
            var asyncRequest = asyncRequestResponse.AsyncRequest;            

            while (asyncRequest.AsyncResponse is null)
            {
                Task.Delay(2000);
                var asyncStatusRequest = new PhraseTmsRequest($"/api2/v1/async/{asyncRequest.Id}", Method.Get, provider.Value);
                asyncRequest = this.Get<AsyncRequest>(asyncStatusRequest);
                if (asyncRequest is null) return default;
            }

            return asyncRequest;

        }
    }
}
