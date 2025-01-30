using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS;

public static class WebhookLogger
{
    private static string Url = @"https://webhook.site/0854fe68-d189-488d-81a0-594c65a6ec3c";

    public static async Task LogAsync<T>(T obj)
    {
        var restClient = new RestClient(Url);
        var request = new RestRequest(string.Empty, Method.Post)
            .WithJsonBody(obj);

        await restClient.ExecuteAsync(request);
    }
}