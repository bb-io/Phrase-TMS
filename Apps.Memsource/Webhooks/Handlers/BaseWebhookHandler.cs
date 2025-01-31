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

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values)
    {
        try
        {
            var currentRetry = 0;
            await UnsubscribeRecursivelyAsync(authenticationCredentialsProvider, values, currentRetry);

            currentRetry += 1;
            await Task.Delay(10000);
            await UnsubscribeRecursivelyAsync(authenticationCredentialsProvider, values, currentRetry);
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

    private async Task UnsubscribeRecursivelyAsync(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider,
        Dictionary<string, string> values,
        int currentRetry)
    {
        await WebhookLogger.LogAsync(new
        {
            status = $"Called UnsubscribeAsync {currentRetry}",
            values
        });

        var authenticationCredentialsProviders = authenticationCredentialsProvider as AuthenticationCredentialsProvider[] ?? authenticationCredentialsProvider.ToArray();
        var client = new PhraseTmsClient(authenticationCredentialsProviders);
        var getRequest = new PhraseTmsRequest($"/api2/v2/webhooks?name={subEvent}&url={values["payloadUrl"]}",
            Method.Get, authenticationCredentialsProviders);
        var webhooks = await client.ExecuteWithHandling<ResponseWrapper<List<WebhookDto>>>(getRequest);
        var webhookUId = webhooks?.Content.FirstOrDefault()?.UId;

        await WebhookLogger.LogAsync(new
        {
            status = $"After getting webhooks {currentRetry}",
            webhooks,
            webhookUId
        });

        if (webhookUId == null)
            return;
        
        var deleteRequest = new RestRequest($"/api2/v2/webhooks/{webhookUId}", Method.Delete);
        var value = authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value;
        deleteRequest.AddHeader("Authorization", value);
        
        var deleteClient = new PhraseTmsClient(authenticationCredentialsProviders);
        var result = await deleteClient.ExecuteWithHandling(deleteRequest);
        await WebhookLogger.LogAsync(new
        {
            status = $"successfully deleted webhook {currentRetry}",
            result.Content
        });
    }
}