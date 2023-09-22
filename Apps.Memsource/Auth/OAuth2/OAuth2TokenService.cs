﻿using System.Text.Json;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;

namespace Apps.PhraseTMS.Auth.OAuth2;

public class OAuth2TokenService : IOAuth2TokenService
{
    private const string ExpiresAtKeyName = "expires_at";
    private const string TokenUrl = "https://us.cloud.memsource.com/web/oauth/token";

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
        const string grant_type = "authorization_code";

        var bodyParameters = new Dictionary<string, string>
        {
            { "grant_type", grant_type },
            { "client_id", values["client_id"] },
            { "redirect_uri", ApplicationConstants.RedirectUri },
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
        using HttpClient httpClient = new HttpClient();
        using var httpContent = new FormUrlEncodedContent(bodyParameters);
        using var response = await httpClient.PostAsync(url, httpContent, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync();
        var resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent)?.ToDictionary(r => r.Key, r => r.Value?.ToString())
                               ?? throw new InvalidOperationException($"Invalid response content: {responseContent}");
        //var expriresIn = int.Parse(resultDictionary["expires_in"]);
        //var expiresAt = utcNow.AddSeconds(expriresIn);
        //resultDictionary.Add(ExpiresAtKeyName, expiresAt.ToString());
        return resultDictionary;
    }
}