using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.PhraseTMS.Connections
{
    public class OAuth2ConnectionDefinition : IConnectionDefinition
    {
        private const string ApiKeyName = "apiToken";

        public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>()
        {
            //new ConnectionPropertyGroup
            //{
            //    Name = "OAuth2",
            //    AuthenticationType = ConnectionAuthenticationType.OAuth2,
            //    ConnectionUsage = ConnectionUsage.Actions,
            //    ConnectionProperties = new List<ConnectionProperty>()
            //    {
            //        new ConnectionProperty("client_id"),
            //        new ConnectionProperty("redirect_uri"),
            //        new ConnectionProperty("api_endpoint"),
            //    }
            //}, 
            // Api token
            new ConnectionPropertyGroup
            {
                Name = "API Token",
                AuthenticationType = ConnectionAuthenticationType.Undefined,
                ConnectionUsage = ConnectionUsage.Actions,
                ConnectionProperties = new List<ConnectionProperty>()
                {
                    new ConnectionProperty("api_endpoint"),
                    new ConnectionProperty(ApiKeyName)
                }
            }
        };

        public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
        {
            //var token = values.First(v => v.Key == "access_token");
            //yield return new AuthenticationCredentialsProvider(
            //    AuthenticationCredentialsRequestLocation.Header,
            //    "Authorization",
            //    $"Bearer {token.Value}"
            //);
            //var url = values.First(v => v.Key == "api_endpoint");
            //yield return new AuthenticationCredentialsProvider(
            //    AuthenticationCredentialsRequestLocation.None,
            //    "api_endpoint",
            //    url.Value
            //);
            // Api token
            var token = values.First(v => v.Key == ApiKeyName);
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.Header,
                "Authorization",
                $"ApiToken {token.Value}"
            );
            var url = values.First(v => v.Key == "api_endpoint");
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                url.Key,
                url.Value
            );
        }
    }
}