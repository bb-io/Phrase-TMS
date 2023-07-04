using Apps.PhraseTMS.Models.Async;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System.Text;
using Apps.PhraseTMS.Models;
using Newtonsoft.Json;

namespace Apps.PhraseTms
{
    public class PhraseTmsClient : RestClient
    {
        public PhraseTmsClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : 
            base(new RestClientOptions { ThrowOnAnyError = false, BaseUrl = GetUri(authenticationCredentialsProviders) }) { }

        public AsyncRequest? PerformAsyncRequest(PhraseTmsRequest request, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var asyncRequestResponse = this.Execute<AsyncRequestResponse>(request).Data;
            if (asyncRequestResponse is null) return default;
            var asyncRequest = asyncRequestResponse.AsyncRequest;            

            while (asyncRequest.AsyncResponse is null)
            {
                Task.Delay(2000);
                var asyncStatusRequest = new PhraseTmsRequest($"/api2/v1/async/{asyncRequest.Id}", Method.Get, authenticationCredentialsProviders);
                asyncRequest = this.Get<AsyncRequest>(asyncStatusRequest);
                if (asyncRequest is null) return default;
            }

            return asyncRequest;

        }

        public List<AsyncRequest> PerformMultipleAsyncRequest(PhraseTmsRequest request, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var result = new List<AsyncRequest>();
            var asyncRequestResponse = this.Execute<AsyncRequestMultipleResponse>(request).Data;
            if (asyncRequestResponse is null) return default;
            var asyncRequests = asyncRequestResponse.AsyncRequests;

            foreach(var asyncRequest in asyncRequests)
            {
                AsyncRequest asyncRequestSeparate = null;
                while (asyncRequestSeparate?.AsyncResponse is null)
                {
                    Task.Delay(2000);
                    var asyncStatusRequest = new PhraseTmsRequest($"/api2/v1/async/{asyncRequest.AsyncRequest.Id}", Method.Get, authenticationCredentialsProviders);
                    asyncRequestSeparate = this.Get<AsyncRequest>(asyncStatusRequest);
                    if (asyncRequestSeparate is null) return default;                 
                }
                result.Add(asyncRequestSeparate);
            }
            return result;
        }

        private static Uri GetUri(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var url = authenticationCredentialsProviders.First(p => p.KeyName == "url").Value;
            return new Uri(url + "/web");
        }

        public async Task<T> ExecuteWithHandling<T>(Func<Task<RestResponse<T>>> request)
        {
            var response = await request();
            
            if (response.IsSuccessful)
                return response.Data;

            var error = JsonConvert.DeserializeObject<Error>(Encoding.UTF8.GetString(response.RawBytes));
            
            throw new(!string.IsNullOrEmpty(error.ErrorDescription) ? error.ErrorDescription : error.ErrorCode);
        }
        
        public async Task<RestResponse> ExecuteWithHandling(Func<Task<RestResponse>> request)
        {
            var response = await request();
            
            if (response.IsSuccessful)
                return response;

            var error = JsonConvert.DeserializeObject<Error>(Encoding.UTF8.GetString(response.RawBytes));
            
            throw new(!string.IsNullOrEmpty(error.ErrorDescription) ? error.ErrorDescription : error.ErrorCode);
        }
    }
}
