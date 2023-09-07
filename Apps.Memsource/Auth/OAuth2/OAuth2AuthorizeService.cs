using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Utils.Extensions.String;

namespace Apps.PhraseTMS.Auth.OAuth2
{
    public class OAuth2AuthorizeService : IOAuth2AuthorizeService
    {
        public string GetAuthorizationUrl(Dictionary<string, string> values)
        {
            var oauthUrl = values["url"].TrimEnd('/') + "/web/oauth/authorize";
            var parameters = new Dictionary<string, string>
            {
                { "client_id", values["client_id"] },
                { "redirect_uri", ApplicationConstants.RedirectUri },
                { "response_type", "code" },
                { "state", values["state"] }
            };
            return oauthUrl.WithQuery(parameters);
        }
    }
}
