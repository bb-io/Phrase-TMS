using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.PhraseTMS;

public class PhraseTmsRequest : RestRequest
{
    public PhraseTmsRequest(string endpoint, Method method, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(endpoint, method)
    {
        this.AddHeader("Authorization", authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
        this.AddHeader("accept", "*/*");
    }
}