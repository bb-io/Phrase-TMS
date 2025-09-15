using Apps.PhraseTMS.Models.Responses;
using Apps.PhraseTMS.Webhooks.Handlers.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.PhraseTMS.Webhooks.Handlers;

public class BaseWebhookHandler(InvocationContext invocationContext, string subEvent)
    : BaseInvocable(invocationContext), IWebhookEventHandler
{
    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {    
        var client = new PhraseTmsClient(authenticationCredentialsProvider);
        var request = new RestRequest($"/api2/v2/webhooks", Method.Post);
        request.WithJsonBody(new
        {
            events = new[] { subEvent },
            url = values["payloadUrl"],
            name = subEvent
        });

        await client.ExecuteWithHandling(request);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        try
        {
            await IdentifyAndDeleteSubscriptionAsync(authenticationCredentialsProvider, values);
        }
        catch (Exception e)
        {
            var payloadUrl = values.TryGetValue("payloadUrl", out var value) ? value : "N/A";
            InvocationContext.Logger?.LogError($"[PhraseTMSWebhookHandler] Failed to unsubscribe from webhook ({subEvent}): {e.Message}; " +
                                               $"Payload URL: {payloadUrl}", []);
            throw;
        }
    }

    private async Task IdentifyAndDeleteSubscriptionAsync(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        var authenticationCredentialsProviders = authenticationCredentialsProvider as AuthenticationCredentialsProvider[] ?? authenticationCredentialsProvider.ToArray();
        
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var getRequest = new RestRequest($"/api2/v2/webhooks?name={subEvent}&url={values["payloadUrl"]}");
        var webhooks = await client.ExecuteWithHandling<ResponseWrapper<List<WebhookDto>>>(getRequest);
        var webhookUId = webhooks?.Content.FirstOrDefault()?.UId;
        if (webhookUId == null)
        {
            return;
        }

        var deleteRequest = new RestRequest($"/api2/v2/webhooks/{webhookUId}", Method.Delete);
        await client.ExecuteWithHandling(deleteRequest);
    }
}