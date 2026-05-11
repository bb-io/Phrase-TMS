using System.Text.Json;
using Apps.PhraseTMS.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.PhraseTMS.Auth.OAuth2;

public class OAuth2TokenService(InvocationContext invocationContext) 
    : BaseInvocable(invocationContext), IOAuth2TokenService
{
    public bool IsRefreshToken(Dictionary<string, string> values)
    {
        return false;
    }

    public async Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Dictionary<string, string?>> RequestToken(
        string state, 
        string code, 
        Dictionary<string, string> values, 
        CancellationToken cancellationToken)
    {
        var resultDict = new Dictionary<string, string?>();
        
        switch (invocationContext.AuthenticationCredentialsProviders.Get(CredsNames.ConnectionType).Value)
        {
            case ConnectionTypes.OAuth2:
                var codeBodyParameters = new Dictionary<string, string>
                {
                    { "grant_type", "authorization_code" },
                    { "client_id", values["client_id"] },
                    { "redirect_uri", $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode" },
                    { "code", code }
                };
                
                var oauthUrl = values["url"].TrimEnd('/') + "/web/oauth/token";
                resultDict = await RequestToken(codeBodyParameters, oauthUrl, cancellationToken);
                break;
            case ConnectionTypes.ApiToken:
                var tokenBodyParameters = new Dictionary<string, string>
                {
                    { "grant_type", "urn:ietf:params:oauth:grant-type:token-exchange" },
                    { "subject_token", invocationContext.AuthenticationCredentialsProviders.Get(CredsNames.ApiToken).Value },
                    { "subject_token_type", "urn:phrase:params:oauth:token-type:api_token" },
                    { "requested_token_type", "urn:ietf:params:oauth:token-type:access_token" }
                };
                
                var tokenUrl = values["url"].TrimEnd('/') + "/idm/oauth/token";
                resultDict = await RequestToken(tokenBodyParameters, tokenUrl, cancellationToken);
                break;
        }

        return resultDict;
    }

    public Task RevokeToken(Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }

    private async Task<Dictionary<string, string>> RequestToken(Dictionary<string, string> bodyParameters, string url, CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;
        using HttpClient httpClient = new();
        using var httpContent = new FormUrlEncodedContent(bodyParameters);
        using var response = await httpClient.PostAsync(url, httpContent, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync();
        var resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent)?.ToDictionary(r => r.Key, r => r.Value?.ToString())
                               ?? throw new InvalidOperationException($"Invalid response content: {responseContent}");
        
        return resultDictionary;
    }
}