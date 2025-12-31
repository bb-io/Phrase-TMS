namespace Apps.PhraseTMS.Constants;

public static class ConnectionTypes
{
    public const string OAuth2 = "OAuth2";
    public const string Credentials = "Credentials";

    public static readonly IEnumerable<string> SupportedConnectionTypes = [OAuth2, Credentials];
}
