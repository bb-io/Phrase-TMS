
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
    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var client = new PhraseTmsClient(authenticationCredentialsProvider);
        var request = new PhraseTmsRequest($"/api2/v2/webhooks", Method.Post, authenticationCredentialsProvider);
        request.WithJsonBody(new
        {
            events = new[] { subEvent },
            url = values["payloadUrl"],
            name = subEvent
        });
        
        await client.ExecuteWithHandling(request);

        await WebhookLogger.LogAsync(new
        {
            status = "successfully subscribed",
            events = new[] { subEvent },
            url = values["payloadUrl"],
            name = subEvent
        });
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        try
        {
            await WebhookLogger.LogAsync(new
            {
                status = "Called UnsubscribeAsync",
                values
            });

            var client = new PhraseTmsClient(authenticationCredentialsProvider);
            var getRequest = new PhraseTmsRequest($"/api2/v2/webhooks?name={subEvent}&url={values["payloadUrl"]}",
                Method.Get, authenticationCredentialsProvider);
            var webhooks = await client.ExecuteWithHandling<ResponseWrapper<List<WebhookDto>>>(getRequest);
            var webhookUId = webhooks?.Content.FirstOrDefault()?.UId;

            await WebhookLogger.LogAsync(new
            {
                status = "After getting webhooks",
                webhooks,
                webhookUId
            });

            if (webhookUId == null)
                return;

            var deleteRequest = new PhraseTmsRequest($"/api2/v2/webhooks/{webhookUId}", Method.Delete,
                authenticationCredentialsProvider);
            var result = await client.ExecuteWithHandling(deleteRequest);

            await WebhookLogger.LogAsync(new
            {
                status = "successfully unsubscribed",
                result.Content,
                result
            });
            
            var client2 = new PhraseTmsClient(authenticationCredentialsProvider);
            var getRequest2 = new PhraseTmsRequest($"/api2/v2/webhooks?name={subEvent}&url={values["payloadUrl"]}",
                Method.Get, authenticationCredentialsProvider);
            var webhooks2 = await client2.ExecuteWithHandling<ResponseWrapper<List<WebhookDto>>>(getRequest2);
            var webhookUId2 = webhooks?.Content.FirstOrDefault()?.UId;

            await WebhookLogger.LogAsync(new
            {
                status = "After getting webhooks p2",
                webhooks2,
                webhookUId2
            });

            if (webhookUId2 == null)
            {
                await WebhookLogger.LogAsync(new
                {
                    status = "webhookUId2 is null",
                });
                return;
            }

            var deleteRequest2 = new PhraseTmsRequest($"/api2/v2/webhooks/{webhookUId}", Method.Delete,
                authenticationCredentialsProvider);
            var result2 = await client.ExecuteWithHandling(deleteRequest2);
            await WebhookLogger.LogAsync(new
            {
                status = "successfully unsubscribed p2",
                result2.Content,
                result2
            });
        }
        catch (Exception e)
        {
            await WebhookLogger.LogAsync(new
            {
                status = "failed",
                message = e.Message,
                stack_trace = e.StackTrace
            });
            
            throw;
        }
    }
}