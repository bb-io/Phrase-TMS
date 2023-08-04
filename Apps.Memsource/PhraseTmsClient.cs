using Apps.PhraseTMS.Extension;
using Apps.PhraseTMS.Models;
using Apps.PhraseTMS.Models.Async;
using Apps.PhraseTMS.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.PhraseTMS
{
    public class PhraseTmsClient : RestClient
    {
        public PhraseTmsClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) :
            base(new RestClientOptions
                { ThrowOnAnyError = false, BaseUrl = GetUri(authenticationCredentialsProviders) })
        {
        }

        public AsyncRequest? PerformAsyncRequest(PhraseTmsRequest request,
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var asyncRequestResponse = this.Execute<AsyncRequestResponse>(request).Data;
            if (asyncRequestResponse is null) return default;
            var asyncRequest = asyncRequestResponse.AsyncRequest;

            while (asyncRequest.AsyncResponse is null)
            {
                Task.Delay(2000);
                var asyncStatusRequest = new PhraseTmsRequest($"/api2/v1/async/{asyncRequest.Id}", Method.Get,
                    authenticationCredentialsProviders);
                asyncRequest = this.Get<AsyncRequest>(asyncStatusRequest);
                if (asyncRequest is null) return default;
            }

            return asyncRequest;
        }

        public List<AsyncRequest> PerformMultipleAsyncRequest(PhraseTmsRequest request,
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var result = new List<AsyncRequest>();
            var asyncRequestResponse = this.Execute<AsyncRequestMultipleResponse>(request).Data;
            if (asyncRequestResponse is null) return default;
            var asyncRequests = asyncRequestResponse.AsyncRequests;

            foreach (var asyncRequest in asyncRequests)
            {
                AsyncRequest asyncRequestSeparate = null;
                while (asyncRequestSeparate?.AsyncResponse is null)
                {
                    Task.Delay(2000);
                    var asyncStatusRequest = new PhraseTmsRequest($"/api2/v1/async/{asyncRequest.AsyncRequest.Id}",
                        Method.Get, authenticationCredentialsProviders);
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

        public async Task<T> ExecuteWithHandling<T>(RestRequest request)
        {
            var response = await ExecuteWithHandling(request);

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public async Task<RestResponse> ExecuteWithHandling(RestRequest request)
        {
            var response = await ExecuteAsync(request);

            if (response.IsSuccessful)
                return response;

            throw ConfigureErrorException(response.Content);
        }
        
        public async Task<List<T>> Paginate<T>(RestRequest request)
        {
            var pageNumber = 0;
            int totalPages;
            
            var resource = request.Resource;
            var result = new List<T>();

            do
            {
                request.Resource = resource.WithQuery("pageNumber", pageNumber++.ToString());

                var response = await ExecuteWithHandling<PaginationResponse<T[]>>(request);
                totalPages = response.TotalPages;
                
                result.AddRange(response.Content);
                
            } while (pageNumber < totalPages);

            return result;
        }

        private Exception ConfigureErrorException(string responseContent)
        {
            var error = JsonConvert.DeserializeObject<Error>(responseContent);

            return new($"{error.ErrorDescription}; {error.ErrorCode}");
        }
    }
}