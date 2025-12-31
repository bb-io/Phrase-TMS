using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.PhraseTMS.Connections;

public class OAuth2ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = ConnectionTypes.OAuth2,
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties = new List<ConnectionProperty>
            {
                new(CredsNames.ClientId) { DisplayName = "Client ID" },
                new(CredsNames.Url) 
                { 
                    DisplayName = "Base Url",
                    Description = "Select the base URL according to your Phrase data center",
                    DataItems = 
                    [
                        new("https://cloud.memsource.com/", "EU data center"),
                        new("https://us.cloud.memsource.com/","US data center"),
                        new("https://cloud9.memsource.com/", "Cloud 9")
                    ]
                }
            }
        },
        new()
        {
            Name = ConnectionTypes.Credentials,
            DisplayName = "Credentials",
            ConnectionProperties = new List<ConnectionProperty>
            {
                new(CredsNames.Username) { DisplayName = "Username" },
                new(CredsNames.Password) { DisplayName = "Password", Sensitive = true },
                new(CredsNames.Url) 
                { 
                    DisplayName = "Base Url",
                    Description = "Select the base URL according to your Phrase data center",
                    DataItems =
                    [
                        new("https://cloud.memsource.com/", "EU data center"),
                        new("https://us.cloud.memsource.com/","US data center"),
                        new("https://cloud9.memsource.com/", "Cloud 9")
                    ]
                }
            }
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
    {
        var providers = new List<AuthenticationCredentialsProvider>();

        string? token = values.FirstOrDefault(v => v.Key == CredsNames.AccessToken).Value;
        if (token != null)
            providers.Add(new("Authorization", $"Bearer {token}"));        

        foreach (var kv in values.Where(x => x.Key != CredsNames.AccessToken))
            providers.Add(new AuthenticationCredentialsProvider(kv.Key, kv.Value));

        var connectionType = values[nameof(ConnectionPropertyGroup)] switch
        {
            var ct when ConnectionTypes.SupportedConnectionTypes.Contains(ct) => ct,
            _ => throw new Exception($"Unknown connection type: {values[nameof(ConnectionPropertyGroup)]}")
        };

        providers.Add(new AuthenticationCredentialsProvider(CredsNames.ConnectionType, connectionType));
        return providers;
    }
}