﻿using Blackbird.Applications.Sdk.Common.Authentication;
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
            ConnectionProperties = new List<ConnectionProperty>
            {
                new("client_id"){DisplayName = "Client ID"},
                new("url"){DisplayName="Base Url",
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
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
    {
        var token = values.First(v => v.Key == "access_token");
        yield return new("Authorization", $"Bearer {token.Value}");
        
        var url = values.First(v => v.Key == "url");
        yield return new("url", url.Value);
    }
}