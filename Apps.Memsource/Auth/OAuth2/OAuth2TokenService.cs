using System.Text.Json;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.PhraseTMS.Auth.OAuth2;

public class OAuth2TokenService : BaseInvocable, IOAuth2TokenService
{
    private const string ExpiresAtKeyName = "expires_at";
    private const string TokenUrl = "https://us.cloud.memsource.com/web/oauth/token";

    public OAuth2TokenService(InvocationContext invocationContext) : base(invocationContext)
    {
    }

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
        const string grantType = "authorization_code";

        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", grantType },
            { "client_id", values["client_id"] },
            { "redirect_uri", $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode" },
            { "code", code }
        };
        var url = values["url"].TrimEnd('/') + "/web/oauth/token";
        return await RequestToken(bodyParameters, url, cancellationToken);
    }

    public Task RevokeToken(Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }

    private async Task<Dictionary<string, string>> RequestToken(Dictionary<string, string> bodyParameters, string url, CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;
        InvocationContext.Logger?.LogInformation(
             "[PhraseTMS OAuth2 RequestToken] Sending request to {0}",
             new object[] { url });
        try
        {
            using HttpClient httpClient = new();
            using var httpContent = new FormUrlEncodedContent(bodyParameters);
            using var response = await httpClient.PostAsync(url, httpContent, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();

            InvocationContext.Logger?.LogInformation(
                "[PhraseTMS OAuth2 RequestToken] Received response from {0}. Status: {1}, Content: {2}",
                new object[] { url, response.StatusCode, responseContent });

            var resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent)?.ToDictionary(r => r.Key, r => r.Value?.ToString())
                                   ?? throw new InvalidOperationException($"Invalid response content: {responseContent}");

            //var expriresIn = int.Parse(resultDictionary["expires_in"]);
            //var expiresAt = utcNow.AddSeconds(expriresIn);
            //resultDictionary.Add(ExpiresAtKeyName, expiresAt.ToString());
            return resultDictionary;
        }
        catch (JsonException ex)
        {
            InvocationContext.Logger?.LogError(
                "[PhraseTMS OAuth2 RequestToken] Failed to deserialize response from {0}. Error: {1}",
                new object[] { url, ex.Message });
            throw;
        }
        catch (Exception ex)
        {
            InvocationContext.Logger?.LogError(
                "[PhraseTMS OAuth2 RequestToken] Error during request to {0}. Error: {1}",
                new object[] { url, ex.Message });
            throw;
        }
    }
}