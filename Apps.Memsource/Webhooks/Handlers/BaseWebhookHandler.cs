﻿
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
            await client.ExecuteWithHandling(deleteRequest);

            await WebhookLogger.LogAsync(new
            {
                status = "successfully unsubscribed",
                events = new[] { subEvent },
                url = values["payloadUrl"],
                name = subEvent
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