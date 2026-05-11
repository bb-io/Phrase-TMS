namespace Apps.PhraseTMS.Constants;

public static class ConnectionTypes
{
    public const string ApiToken = "ApiToken";
    public const string OAuth2 = "OAuth2";
    public const string Credentials = "Credentials";

    public static readonly IEnumerable<string> SupportedConnectionTypes = [ApiToken, OAuth2, Credentials];
}
