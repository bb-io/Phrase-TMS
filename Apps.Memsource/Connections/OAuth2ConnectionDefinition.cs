using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.PhraseTMS.Connections;

public class OAuth2ConnectionDefinition : IConnectionDefinition
{
    private const string ApiKeyName = "apiToken";

    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = "OAuth2",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionUsage = ConnectionUsage.Actions,
            ConnectionProperties = new List<ConnectionProperty>
            {
                new("client_id"){DisplayName = "Client ID"},
                new("url"){DisplayName="Base Url",
                    Description = "Select the base URL according to your Phrase data center",
                    DataItems= [new("https://cloud.memsource.com/", "EU data center"),
                                new("https://us.cloud.memsource.com/","US data center")]
                }
            }
        }, 
        // Api token
        //new ConnectionPropertyGroup
        //{
        //    Name = "API Token",
        //    AuthenticationType = ConnectionAuthenticationType.Undefined,
        //    ConnectionUsage = ConnectionUsage.Actions,
        //    ConnectionProperties = new List<ConnectionProperty>()
        //    {
        //        new ConnectionProperty("url"),
        //        new ConnectionProperty(ApiKeyName)
        //    }
        //}
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
    {
        var token = values.First(v => v.Key == "access_token");
        yield return new(
            AuthenticationCredentialsRequestLocation.Header,
            "Authorization",
            $"Bearer {token.Value}"
        );
        var url = values.First(v => v.Key == "url");
        yield return new(
            AuthenticationCredentialsRequestLocation.None,
            "url",
            url.Value
        );
        // Api token
        //var token = values.First(v => v.Key == ApiKeyName);
        //yield return new AuthenticationCredentialsProvider(
        //    AuthenticationCredentialsRequestLocation.Header,
        //    "Authorization",
        //    $"ApiToken {token.Value}"
        //);
        //var url = values.First(v => v.Key == "url");
        //yield return new AuthenticationCredentialsProvider(
        //    AuthenticationCredentialsRequestLocation.None,
        //    url.Key,
        //    url.Value
        //);
    }
}