using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS;

public static class WebhookLogger
{
    private static string Url = @"https://webhook.site/a04a2106-f22b-468e-8833-2196184a06e0";

    public static async Task LogAsync<T>(T obj)
    {
        var restClient = new RestClient(Url);
        var request = new RestRequest(string.Empty, Method.Post)
            .WithJsonBody(obj);

        await restClient.ExecuteAsync(request);
    }
}