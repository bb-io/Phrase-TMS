
using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Webhooks.Handlers.Models;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks.Handlers;

public class BaseWebhookHandler : IWebhookEventHandler
{

    private string _subscriptionEvent;

    public BaseWebhookHandler(string subEvent)
    {
        _subscriptionEvent = subEvent;
    }

    public Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProvider);
        var request = new PhraseTmsRequest($"/api2/v2/webhooks", Method.Post, authenticationCredentialsProvider);
        request.WithJsonBody(new
        {
            events = new[] { _subscriptionEvent },
            url = values["payloadUrl"],
            name = _subscriptionEvent
        });
            
        return client.ExecuteWithHandling(request);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProvider);
        var getRequest = new PhraseTmsRequest($"/api2/v2/webhooks?name={_subscriptionEvent}&url={values["payloadUrl"]}", Method.Get, authenticationCredentialsProvider);
        var webhooks = await client.ExecuteWithHandling<ResponseWrapper<List<WebhookDto>>>(getRequest);
        var webhookUId = webhooks?.Content.FirstOrDefault()?.UId;

        if (webhookUId == null)
            return;

        var deleteRequest = new PhraseTmsRequest($"/api2/v2/webhooks/{webhookUId}", Method.Delete, authenticationCredentialsProvider);
        await client.ExecuteWithHandling(deleteRequest);
    }
}